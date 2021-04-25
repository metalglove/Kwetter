using Kwetter.Services.Common.API.CQRS;
using MediatR;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.TimelineService.API.Application.Queries.KweetTimelineQuery
{
    /// <summary>
    /// Represents the <see cref="KweetTimelineQueryHandler"/> class.
    /// </summary>
    public sealed class KweetTimelineQueryHandler : IRequestHandler<KweetTimelineQuery, QueryResponse<IEnumerable<KweetDto>>>
    {
        private readonly IRedisCacheClient _redisCacheClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetTimelineQueryHandler"/> class.
        /// </summary>
        /// <param name="redisCacheClient">The redis client.</param>
        public KweetTimelineQueryHandler(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;
        }

        /// <summary>
        /// Handles the kweet time line query asynchronously.
        /// </summary>
        /// <param name="request">The kweet time line query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The query response.</returns>
        public Task<QueryResponse<IEnumerable<KweetDto>>> Handle(KweetTimelineQuery request, CancellationToken cancellationToken)
        {
            _redisCacheClient.Db0.HashScan
            throw new NotImplementedException();
        }
    }
}
