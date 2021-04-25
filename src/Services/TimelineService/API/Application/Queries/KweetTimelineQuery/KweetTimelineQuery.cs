using Kwetter.Services.Common.API.CQRS;
using MediatR;
using System;
using System.Collections.Generic;

namespace Kwetter.Services.TimelineService.API.Application.Queries.KweetTimelineQuery
{
    /// <summary>
    /// Represents the <see cref="KweetTimelineQuery"/> record.
    /// </summary>
    public record KweetTimelineQuery : IRequest<QueryResponse<IEnumerable<KweetDto>>>
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the page number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets and sets the page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets and sets the last known kweet id.
        /// </summary>
        public Guid? LastKnownKweetId { get; set; }
    }
}
