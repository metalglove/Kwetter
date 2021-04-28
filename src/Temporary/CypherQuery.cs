using NRedisGraph;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Redis
{
    public class CypherQuery
    {
        private readonly StringBuilder _stringBuilder;
        private readonly RedisGraph _redisGraph;
        private readonly string _graphId;

        public CypherQuery(RedisGraph redisGraph, string graphId)
        {
            _redisGraph = redisGraph;
            _graphId = graphId;
            _stringBuilder = new StringBuilder();
        }

        public async Task<ResultSet> ExecuteAsync(Dictionary<string, object> parameters)
        {
            string query = _stringBuilder.ToString();
            return await _redisGraph.QueryAsync(_graphId, query, parameters);
        }

        public async Task<ResultSet> ExecuteAsync()
        {
            string query = _stringBuilder.ToString();
            return await _redisGraph.QueryAsync(_graphId, query);
        }

        public CypherQuery Match(string match)
        {
            _stringBuilder.Append($"MATCH {match} ");
            return this;
        }

        public CypherQuery Where(string where)
        {
            _stringBuilder.Append($"WHERE ({where}) ");
            return this;
        }

        public CypherQuery Create(string create)
        {
            _stringBuilder.Append($"CREATE {create} ");
            return this;
        }

        public CypherQuery Return(string keys)
        {
            _stringBuilder.Append($"RETURN {keys} ");
            return this;
        }

        public CypherQuery OrderBy(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("Order by key is empty");
            _stringBuilder.Append($"ORDER BY {key} ");
            return this;
        }

        public CypherQuery OrderByDescending(string key)
        {
            OrderBy(key);
            _stringBuilder.Append($"DESC ");
            return this;
        }

        public CypherQuery OrderByAscending(string key)
        {
            OrderBy(key);
            _stringBuilder.Append($"ASC ");
            return this;
        }

        public CypherQuery Skip(int skip)
        {
            _stringBuilder.Append($"SKIP {skip} ");
            return this;
        }

        public CypherQuery Limit(int limit)
        {
            _stringBuilder.Append($"LIMIT {limit} ");
            return this;
        }
    }
}
