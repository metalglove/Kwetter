using Kwetter.Services.Common.Infrastructure.Redis;
using Kwetter.Services.TimelineService.Domain;
using Microsoft.Extensions.Logging;
using NRedisGraph;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.Services.TimelineService.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="TimelineGraphStore"/> class.
    /// </summary>
    public sealed class TimelineGraphStore : ITimelineGraphStore
    {
        private readonly ILogger<TimelineGraphStore> _logger;
        private readonly RedisGraph _redisGraph;
        private const string GRAPH_ID = "Kwetter";

        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineGraphStore"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="redisDatabase">The redis database.</param>
        public TimelineGraphStore(ILogger<TimelineGraphStore> logger, IRedisDatabase redisDatabase)
        {
            _redisGraph = new RedisGraph(redisDatabase.Database);
            _logger = logger;
        }

        /// <inheritdoc cref="ITimelineGraphStore.CreateFollowerAsync(Follow)"/>
        public async Task<bool> CreateFollowerAsync(Follow follow)
        {
            Dictionary<string, object> parameters = new()
            {
                { "followerId", follow.FollowerId.ToString().ToLower() },
                { "followingId", follow.FollowingId.ToString().ToLower() },
                { "followedDateTime", follow.FollowDateTime }
            };
            // MATCH (a:User), (b:User) WHERE (a.id = $followerId AND b.id = $followingId) CREATE (a)-[:FOLLOWS {followedDateTime: $followedDateTime}]->(b)
            ResultSet followerCreationResultSet = await new CypherQuery(_redisGraph, GRAPH_ID)
                .Match("(a:User), (b:User)")
                .Where("a.id = $followerId AND b.id = $followingId")
                .Create("(a)-[:FOLLOWS {followedDateTime: $followedDateTime}]->(b)")
                .ExecuteAsync(parameters);
            LogExectionTime(followerCreationResultSet.Statistics);
            return followerCreationResultSet.Statistics.RelationshipsCreated == 1;
        }

        /// <inheritdoc cref="ITimelineGraphStore.CreateKweetAsync(Kweet)"/>
        public async Task<bool> CreateKweetAsync(Kweet kweet)
        {
            Dictionary<string, object> parameters = new()
            {
                { "kweetId", kweet.Id.ToString().ToLower() },
                { "userId", kweet.UserId.ToString().ToLower() },
                { "message", kweet.Message },
                { "createdDateTime", kweet.CreatedDateTime }
            };
            // MATCH (a:User) WHERE (a.id = $userId) CREATE (a)<-[:KWEETED_BY]-(:Kweet {id: $kweetId, message: $message, createdDateTime: $createdDateTime})
            ResultSet kweetCreationResultSet = await new CypherQuery(_redisGraph, GRAPH_ID)
                .Match("(a:User)")
                .Where("a.id = $userId")
                .Create("(a)<-[:KWEETED_BY]-(:Kweet {id: $kweetId, message: $message, createdDateTime: $createdDateTime})")
                .ExecuteAsync(parameters);
            LogExectionTime(kweetCreationResultSet.Statistics);
            return kweetCreationResultSet.Statistics.RelationshipsCreated == 1 &&
                kweetCreationResultSet.Statistics.NodesCreated == 1;
        }

        /// <inheritdoc cref="ITimelineGraphStore.CreateKweetLikeAsync(KweetLike)"/>
        public async Task<bool> CreateKweetLikeAsync(KweetLike kweetLike)
        {
            Dictionary<string, object> parameters = new()
            {
                { "kweetLikeId", kweetLike.Id.ToString().ToLower() },
                { "kweetId", kweetLike.KweetId.ToString().ToLower() },
                { "userId", kweetLike.UserId.ToString().ToLower() },
                { "likedDateTime", kweetLike.LikedDateTime }
            };
            // MATCH (a:User), (b:Kweet) WHERE (a.id = $userId AND b.id = $kweetId) CREATE (a)<-[:LIKED_BY {id: $kweetLikeId, likedDateTime: $likedDateTime}]-(b)
            ResultSet kweetLikeCreationResultSet = await new CypherQuery(_redisGraph, GRAPH_ID)
                .Match("(a:User), (b:Kweet)")
                .Where("(a.id = $userId AND b.id = $kweetId)")
                .Create("(a)<-[:LIKED_BY {id: $kweetLikeId, likedDateTime: $likedDateTime}]-(b)")
                .ExecuteAsync(parameters);
            LogExectionTime(kweetLikeCreationResultSet.Statistics);
            return kweetLikeCreationResultSet.Statistics.RelationshipsCreated == 1;
        }

        /// <inheritdoc cref="ITimelineGraphStore.CreateUserAsync(User)"/>
        public async Task<bool> CreateUserAsync(User user)
        {
            Dictionary<string, object> parameters = new()
            {
                { "userId", user.Id.ToString().ToLower() },
                { "userDisplayName", user.UserDisplayName }
            };
            // CREATE (:User {id: $userId, userDisplayName: $userDisplayName})
            ResultSet userCreationResultSet = await new CypherQuery(_redisGraph, GRAPH_ID)
                .Create("(:User {id: $userId, userDisplayName: $userDisplayName})")
                .ExecuteAsync(parameters);
            LogExectionTime(userCreationResultSet.Statistics);
            return userCreationResultSet.Statistics.NodesCreated == 1;
        }

        /// <inheritdoc cref="ITimelineGraphStore.CreateUserProfileAsync(UserProfile)"/>
        public async Task<bool> CreateUserProfileAsync(UserProfile userProfile)
        {
            Dictionary<string, object> parameters = new()
            {
                { "userId", userProfile.UserId.ToString().ToLower() },
                { "userProfileId", userProfile.UserId.ToString().ToLower() },
                { "description", userProfile.Description },
                { "pictureUrl", userProfile.PictureUrl }
            };
            // MATCH (a:User) WHERE (a.id = $userId) CREATE (a)-[:DESCRIBED_BY]->(:UserProfile {id: $userProfileId, userId: $userId, description: $description, pictureUrl: $pictureUrl})
            ResultSet userCreationResultSet = await new CypherQuery(_redisGraph, GRAPH_ID)
                .Match("(a:User)")
                .Where("a.id = $userId")
                .Create("(a)-[:DESCRIBED_BY]->(:UserProfile {userProfileId: $userProfileId, userId: $userId, description: $description, pictureUrl: $pictureUrl})")
                .ExecuteAsync(parameters);
            LogExectionTime(userCreationResultSet.Statistics);
            return userCreationResultSet.Statistics.RelationshipsCreated == 1 &&
                userCreationResultSet.Statistics.NodesCreated == 1;
        }

        /// <inheritdoc cref="ITimelineGraphStore.GetPaginatedTimelineAsync(Guid,int,int)"/>
        public async Task<Timeline> GetPaginatedTimelineAsync(Guid userId, int pageNumber, int pageSize)
        {
            Dictionary<string, object> parameters = new()
            {
                { "id", userId.ToString().ToLower() }
            };
            // MATCH (:User{id:$id})-[:FOLLOWS]->(user:User)-[:KWEETED_BY]-(kweet:Kweet) RETURN kweet.id, kweet.message, kweet.createdDateTime, user.id
            ResultSet timelineResultSet = await new CypherQuery(_redisGraph, GRAPH_ID)
                .Match("(:User{id:$id})-[:FOLLOWS]->(user:User)-[:KWEETED_BY]-(kweet:Kweet)")
                .Return("kweet.id, kweet.message, kweet.createdDateTime, user.id, user.userDisplayName")
                .OrderByDescending("kweet.createdDateTime")
                .Skip(pageNumber * pageSize)
                .Limit(pageSize)
                .ExecuteAsync(parameters);
            LogExectionTime(timelineResultSet.Statistics);
            Timeline timeline = new();
            timeline.UserId = userId;
            timeline.Kweets = new List<TimelineKweet>();
            timeline.PageNumber = pageNumber;
            timeline.PageSize = pageSize;
            using (IEnumerator<Record> enumerator = timelineResultSet.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Record record = enumerator.Current;
                    TimelineKweet kweet = new();
                    kweet.Id = Guid.Parse(record.GetValue<string>("kweet.id"));
                    kweet.UserId = Guid.Parse(record.GetValue<string>("user.id"));
                    kweet.Message = record.GetValue<string>("kweet.message");
                    kweet.UserDisplayName = record.GetValue<string>("user.userDisplayName");
                    kweet.Liked = record.GetValue<bool>("liked");
                    kweet.LikeCount = record.GetValue<int>("likeCount");
                    kweet.CreatedDateTime = DateTime.Parse(record.GetValue<string>("kweet.createdDateTime"));
                    timeline.Kweets.Add(kweet);
                }
            }
            return timeline;
        }

        private void LogExectionTime(Statistics statistics)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("Redis Query Statistics:");
            stringBuilder.AppendLine($"\tNodes created: {statistics.NodesCreated}");
            stringBuilder.AppendLine($"\tRelationships created: {statistics.RelationshipsCreated}");
            stringBuilder.AppendLine($"\tQuery execution time: {statistics.QueryInternalExecutionTime}");
            _logger.LogInformation(stringBuilder.ToString());
        }
    }
}
