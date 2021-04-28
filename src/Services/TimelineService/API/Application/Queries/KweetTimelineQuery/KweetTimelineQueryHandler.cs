using AutoMapper;
using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.TimelineService.Domain;
using Kwetter.Services.TimelineService.Infrastructure;
using MediatR;
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
        private readonly IMapper _mapper;
        private readonly ITimelineGraphStore _timelineGraphStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetTimelineQueryHandler"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="timelineGraphStore">The timeline graph store.</param>
        public KweetTimelineQueryHandler(IMapper mapper, ITimelineGraphStore timelineGraphStore)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _timelineGraphStore = timelineGraphStore ?? throw new ArgumentNullException(nameof(timelineGraphStore));
        }

        /// <summary>
        /// Handles the kweet time line query asynchronously.
        /// </summary>
        /// <param name="request">The kweet time line query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The query response.</returns>
        public async Task<QueryResponse<IEnumerable<KweetDto>>> Handle(KweetTimelineQuery request, CancellationToken cancellationToken)
        {
            Timeline timeline = await _timelineGraphStore.GetPaginatedTimelineAsync(request.UserId, request.PageNumber, request.PageSize);
            QueryResponse<IEnumerable<KweetDto>> queryResponse = new();
            queryResponse.Data = _mapper.Map<IEnumerable<KweetDto>>(timeline.Kweets);
            queryResponse.Success = true;
            return queryResponse;
        }
    }
}
