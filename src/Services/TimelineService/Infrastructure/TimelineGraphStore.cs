using Kwetter.Services.TimelineService.Domain;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        MATCH (a:User { id: $followerId }), (b:User { id: $followingId }) 
                        CREATE (a)<-[:FOLLOWED_BY {followedDateTime: $followedDateTime}]-(b)",
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
                        MATCH (a:User { id: $userId })
                        CREATE (a)<-[:KWEETED_BY]-(kweet:Kweet {id: $kweetId, message: $message, createdDateTime: $createdDateTime})
                        WITH kweet
                        CALL 
                        {
                            WITH kweet
                            UNWIND ($mentions) AS mentions
                            OPTIONAL MATCH (mention:User {name: mentions })
                            CREATE (kweet)<-[:MENTIONED_IN]-(mention)
                            RETURN true AS ok
                            UNION
                            WITH kweet
                            UNWIND ($tags) AS tags
                            MERGE (tag:Tag { tag: tags })
                            CREATE (kweet)<-[:TAGGED_IN]-(tag)
                            RETURN true AS ok
                        }
                        RETURN ok
                        ",
                        new
                        {
                            kweetId = kweet.Id.ToString(),
                            userId = kweet.UserId.ToString(),
                            message = kweet.Message,
                            createdDateTime = kweet.CreatedDateTime,
                            mentions = kweet.Mentions,
                            tags = kweet.HashTags
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
            return result.Counters.RelationshipsCreated == (1 + kweet.Mentions.Count() + kweet.HashTags.Count()) && result.Counters.NodesCreated >= 1;
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
                        MATCH (a:User { id: $userId }), (b:Kweet { id: $kweetId }) 
                        CREATE (a)<-[:LIKED_BY {likedDateTime: $likedDateTime}]-(b)",
                        new
                        {
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
                        CREATE (:User {id: $userId, displayName: $displayName, name: $userName, profilePictureUrl: $profilePictureUrl})",
                        new
                        {
                            displayName = user.UserDisplayName,
                            userName = user.UserName,
                            userId = user.Id.ToString(),
                            profilePictureUrl = user.UserProfilePictureUrl
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

        /// <inheritdoc cref="ITimelineGraphStore.GetPaginatedTimelineAsync(Guid,uint,uint)"/>
        public async Task<Timeline> GetPaginatedTimelineAsync(Guid userId, uint pageNumber, uint pageSize)
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
                            MATCH (:User {id: $userId})<-[:FOLLOWED_BY]-(user:User)<-[:KWEETED_BY]-(kweet:Kweet)
                            RETURN user, kweet
                            UNION
                            MATCH (user:User {id: $userId})<-[:KWEETED_BY]-(kweet:Kweet)
                            RETURN user, kweet
                            UNION
                            MATCH (:User {id: $userId})-[:MENTIONED_IN]->(kweet:Kweet)-[:KWEETED_BY]->(user:User)
                            RETURN user, kweet
                        }
                        WITH user, kweet
                        ORDER BY kweet.createdDateTime DESC
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
                        RETURN user.id, user.name, user.displayName, user.profilePictureUrl, kweet.createdDateTime, kweet.id, kweet.message, count(liker) as likes, liked",
                        new
                        {
                            userId = userId.ToString(),
                            skip = (int)pageNumber * pageSize,
                            limit = (int)pageSize
                        }
                    );
                    return await cursor.ToListAsync(record => new TimelineKweet()
                    {
                        Id = Guid.Parse(record["kweet.id"].As<string>()),
                        UserId = Guid.Parse(record["user.id"].As<string>()),
                        LikeCount = record["likes"].As<int>(),
                        Message = record["kweet.message"].As<string>(),
                        UserDisplayName = record["user.displayName"].As<string>(),
                        UserName = record["user.name"].As<string>(),
                        CreatedDateTime = DateTime.Parse(record["kweet.createdDateTime"].As<string>()),
                        UserProfilePictureUrl = record["user.profilePictureUrl"].As<string>(),
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

        /// <inheritdoc cref="ITimelineGraphStore.DeleteKweetLikeAsync(Guid,Guid)"/>
        public async Task<bool> DeleteKweetLikeAsync(Guid userId, Guid kweetId)
        {
            IAsyncSession session = _driver.AsyncSession((c) => c.WithDatabase(DATABASE));
            IResultSummary result;
            try
            {
                result = await session.WriteTransactionAsync(async transaction =>
                {
                    IResultCursor cursor = await transaction.RunAsync(@"
                        MATCH (:User { id: $userId })<-[like:LIKED_BY]-(:Kweet { id: $kweetId }) 
                        DELETE like",
                        new
                        {
                            kweetId = kweetId.ToString(),
                            userId = userId.ToString()
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
            return result.Counters.RelationshipsDeleted == 1;
        }

        /// <inheritdoc cref="ITimelineGraphStore.UpdateUserDisplayNameAsync(Guid,string)"/>
        public async Task<bool> UpdateUserDisplayNameAsync(Guid userId, string userDisplayName)
        {
            IAsyncSession session = _driver.AsyncSession((c) => c.WithDatabase(DATABASE));
            IResultSummary result;
            try
            {
                result = await session.WriteTransactionAsync(async transaction =>
                {
                    IResultCursor cursor = await transaction.RunAsync(@"
                        MATCH (user:User { id: $userId }) 
                        SET user.displayName = $displayName",
                        new
                        {
                            displayName = userDisplayName,
                            userId = userId.ToString()
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
            return result.Counters.PropertiesSet == 1;
        }

        /// <inheritdoc cref="ITimelineGraphStore.UpdateUserProfilePictureUrlAsync(Guid,string)"/>
        public async Task<bool> UpdateUserProfilePictureUrlAsync(Guid userId, string userProfilePictureUrl)
        {
            IAsyncSession session = _driver.AsyncSession((c) => c.WithDatabase(DATABASE));
            IResultSummary result;
            try
            {
                result = await session.WriteTransactionAsync(async transaction =>
                {
                    IResultCursor cursor = await transaction.RunAsync(@"
                        MATCH (user:User { id: $userId })
                        SET user.profilePictureUrl = $profilePictureUrl",
                        new
                        {
                            profilePictureUrl = userProfilePictureUrl,
                            userId = userId.ToString()
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
            return result.Counters.PropertiesSet == 1;
        }

        /// <inheritdoc cref="ITimelineGraphStore.DeleteFollowerAsync(Guid,Guid)"/>
        public async Task<bool> DeleteFollowerAsync(Guid followerId, Guid followingId)
        {
            IAsyncSession session = _driver.AsyncSession((c) => c.WithDatabase(DATABASE));
            IResultSummary result;
            try
            {
                result = await session.WriteTransactionAsync(async transaction =>
                {
                    IResultCursor cursor = await transaction.RunAsync(@"
                        MATCH (:User { id: $followingId })-[follow:FOLLOWED_BY]->(:User { id: $followerId }) 
                        DELETE follow",
                        new
                        {
                            followingId = followingId.ToString(),
                            followerId = followerId.ToString()
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
            return result.Counters.RelationshipsDeleted == 1;
        }
    }
}
