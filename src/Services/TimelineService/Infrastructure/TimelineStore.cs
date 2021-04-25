using StackExchange.Redis;

namespace Kwetter.Services.TimelineService.Infrastructure
{
    public sealed class TimelineStore
    {
        private readonly IDatabaseAsync _database;

        public TimelineStore(IDatabaseAsync database)
        {
            _database = database;
            //ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            //_database.get
        }


        // [SortedSet] Storing followers -> followers:{userId} with unix time with value date time

        // [SortedSet] Storing followings -> followings:{userId} with unix time with value date time

        // [SortedSet] Storing kweets is a fanout to all timelines:
        //      First get all followers -> followers:{userid}
        //      Add kweet to all followers timeline -> timelines:{userId} sorted by unix time with value kweet id

        // docker run -v redisinsight:/db --network host redislabs/redisinsight:latest
        // docker run -p 6379:6379 redislabs/redisgraph
    }
}
