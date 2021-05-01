using Kwetter.Services.TimelineService.Domain;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kwetter.Services.TimelineService.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="TimelineGraphStore"/> class.
    /// </summary>
    public sealed class TimelineGraphStore : ITimelineGraphStore
    {
        private readonly ILogger<TimelineGraphStore> _logger;
        private readonly IDriver _driver;
        private const string DATABASE = "neo4j";

        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineGraphStore"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="driver">The neo4j driver.</param>
        public TimelineGraphStore(ILogger<TimelineGraphStore> logger, IDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            _logger = logger ?? throw new ArgumentNullException(nameof(driver));
        }

        /// <inheritdoc cref="ITimelineGraphStore.CreateFollowerAsync(Follow)"/>
        public async Task<bool> CreateFollowerAsync(Follow follow)
        {
            IAsyncSession session = _driver.AsyncSession((c) => c.WithDatabase(DATABASE));
            IResultSummary result;
            try
            {
                result = await session.WriteTransactionAsync(async transaction =>
                {
                    IResultCursor cursor = await transaction.RunAsync(@"
                        MATCH (a:User), (b:User) 
                        WHERE (a.id = $followerId AND b.id = $followingId) 
                        CREATE (a)-[:FOLLOWS {followedDateTime: $followedDateTime}]->(b)",
                        new { 
                            followerId = follow.FollowerId.ToString(),
                            followingId = follow.FollowingId.ToString(),
                            followedDateTime = follow.FollowDateTime
                        }
                    );
                    return await cursor.ConsumeAsync();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
            return result.Counters.RelationshipsCreated == 1;
        }

        /// <inheritdoc cref="ITimelineGraphStore.CreateKweetAsync(Kweet)"/>
        public async Task<bool> CreateKweetAsync(Kweet kweet)
        {
            IAsyncSession session = _driver.AsyncSession((c) => c.WithDatabase(DATABASE));
            IResultSummary result;
            try
            {
                result = await session.WriteTransactionAsync(async transaction =>
                {
                    IResultCursor cursor = await transaction.RunAsync(@"
                        MATCH (a:User) 
                        WHERE (a.id = $userId) 
                        CREATE (a)<-[:KWEETED_BY]-(:Kweet {id: $kweetId, message: $message, createdDateTime: $createdDateTime})",
                        new
                        {
                            kweetId = kweet.Id.ToString(),
                            userId = kweet.UserId.ToString(),
                            message = kweet.Message,
                            createdDateTime = kweet.CreatedDateTime
                        }
                    );
                    return await cursor.ConsumeAsync();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
            return result.Counters.RelationshipsCreated == 1 && result.Counters.NodesCreated == 1;
        }

        /// <inheritdoc cref="ITimelineGraphStore.CreateKweetLikeAsync(KweetLike)"/>
        public async Task<bool> CreateKweetLikeAsync(KweetLike kweetLike)
        {
            IAsyncSession session = _driver.AsyncSession((c) => c.WithDatabase(DATABASE));
            IResultSummary result;
            try
            {
                result = await session.WriteTransactionAsync(async transaction =>
                {
                    IResultCursor cursor = await transaction.RunAsync(@"
                        MATCH (a:User), (b:Kweet) 
                        WHERE (a.id = $userId AND b.id = $kweetId) 
                        CREATE (a)<-[:LIKED_BY {id: $kweetLikeId, likedDateTime: $likedDateTime}]-(b)",
                        new
                        {
                            kweetLikeId = kweetLike.Id.ToString(),
                            kweetId = kweetLike.KweetId.ToString(),
                            userId = kweetLike.UserId.ToString(),
                            likedDateTime = kweetLike.LikedDateTime
                        }
                    );
                    return await cursor.ConsumeAsync();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
            return result.Counters.RelationshipsCreated == 1;
        }

        /// <inheritdoc cref="ITimelineGraphStore.CreateUserAsync(User)"/>
        public async Task<bool> CreateUserAsync(User user)
        {
            IAsyncSession session = _driver.AsyncSession((c) => c.WithDatabase(DATABASE));
            IResultSummary result;
            try
            {
                result = await session.WriteTransactionAsync(async transaction =>
                {
                    IResultCursor cursor = await transaction.RunAsync(@"
                        CREATE (:User {id: $userId, userDisplayName: $userDisplayName})",
                        new
                        {
                            userDisplayName = user.UserDisplayName,
                            userId = user.Id.ToString(),
                        }
                    );
                    return await cursor.ConsumeAsync();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
            return result.Counters.NodesCreated == 1;
        }

        /// <inheritdoc cref="ITimelineGraphStore.CreateUserProfileAsync(UserProfile)"/>
        public async Task<bool> CreateUserProfileAsync(UserProfile userProfile)
        {
            IAsyncSession session = _driver.AsyncSession((c) => c.WithDatabase(DATABASE));
            IResultSummary result;
            try
            {
                result = await session.WriteTransactionAsync(async transaction =>
                {
                    IResultCursor cursor = await transaction.RunAsync(@"
                        MATCH (a:User) 
                        WHERE (a.id = $userId) 
                        CREATE (a)-[:DESCRIBED_BY]->(:UserProfile {id: $userProfileId, userId: $userId, description: $description, pictureUrl: $pictureUrl})",
                        new
                        {
                            userId = userProfile.UserId.ToString(),
                            userProfileId = userProfile.UserId.ToString(),
                            description = userProfile.Description,
                            pictureUrl = userProfile.PictureUrl
                        }
                    );
                    return await cursor.ConsumeAsync();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
            return result.Counters.NodesCreated == 1 && result.Counters.RelationshipsCreated == 1;
        }

        /// <inheritdoc cref="ITimelineGraphStore.GetPaginatedTimelineAsync(Guid,int,int)"/>
        public async Task<Timeline> GetPaginatedTimelineAsync(Guid userId, int pageNumber, int pageSize)
        {
            IAsyncSession session = _driver.AsyncSession((c) => c.WithDatabase(DATABASE));
            Timeline timeline = new();
            timeline.UserId = userId;
            timeline.Kweets = new List<TimelineKweet>();
            timeline.PageNumber = pageNumber;
            timeline.PageSize = pageSize;
            try
            {
                timeline.Kweets = await session.ReadTransactionAsync(async transaction =>
                {
                    IResultCursor cursor = await transaction.RunAsync(@"
                        CALL {
                            MATCH (:User {id: $userId})-[:FOLLOWS]->(user:User)<-[:KWEETED_BY]-(kweet:Kweet)
                            RETURN user, kweet
                            UNION
                            MATCH (user:User {id: $userId})<-[:KWEETED_BY]-(kweet:Kweet)
                            RETURN user, kweet
                        }
                        WITH user, kweet
                        SKIP $skip
                        LIMIT $limit
                        CALL {
                            WITH kweet
                            OPTIONAL MATCH (liker:User)<-[:LIKED_BY]-(kweet)
                            RETURN liker
                        }
                        CALL {
                            WITH kweet
                            OPTIONAL MATCH (i:User {id: $userId})<-[:LIKED_BY]-(kweet)
                            RETURN (i.id IS NOT NULL) as liked
                        }
                        RETURN user.id, user.userDisplayName, kweet.createdDateTime, kweet.id, kweet.message, count(liker) as likes, liked",
                        new
                        {
                            userId = userId.ToString(),
                            skip = pageNumber * pageSize,
                            limit = pageSize
                        }
                    );
                    return await cursor.ToListAsync(record => new TimelineKweet()
                    {
                        Id = Guid.Parse(record["kweet.id"].As<string>()),
                        UserId = Guid.Parse(record["user.id"].As<string>()),
                        LikeCount = record["likes"].As<int>(),
                        Message = record["kweet.message"].As<string>(),
                        UserDisplayName = record["user.userDisplayName"].As<string>(),
                        CreatedDateTime = DateTime.Parse(record["kweet.createdDateTime"].As<string>()),
                        Liked = record["liked"].As<bool>(),
                    });
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
            return timeline;
        }
    }
}
