using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.TimelineService.API.Application.Queries;
using Kwetter.Services.TimelineService.API.Application.Queries.KweetTimelineQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kwetter.Services.TimelineService.API.Controllers
{
    /// <summary>
    /// Represents the <see cref="TimelineController"/> class.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TimelineController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public TimelineController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the kweet timeline for the authenticated user asynchronously.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="lastKnownKweetId">The last known kweet id.</param>
        /// <returns>Returns the command response.</returns>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetKweetTimelineAsync(int pageNumber, int pageSize, Guid? lastKnownKweetId)
        {
            // TODO: Get user id from claims

            KweetTimelineQuery kweetTimelineQuery = new()
            {
                UserId = Guid.NewGuid(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                LastKnownKweetId = lastKnownKweetId
            };
            QueryResponse<IEnumerable<KweetDto>> queryResponse = await _mediator.Send(kweetTimelineQuery);
            return queryResponse.Success
                ? new OkObjectResult(queryResponse)
                : new BadRequestObjectResult(queryResponse);
        }
    }
}
