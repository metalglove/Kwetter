using Kwetter.Services.AuthorizationService.API.Application.Commands.ClaimsCommand;
using Kwetter.Services.AuthorizationService.API.Application.Queries.VerifyUserNameUniquenessQuery;
using Kwetter.Services.Common.API.CQRS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.API.Controllers
{
    /// <summary>
    /// Represents the <see cref="AuthorizationController"/> class.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public AuthorizationController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Special endpoint for claims request when using the firebase api.
        /// </summary>
        /// <param name="claimsCommand">The claims command.</param>
        /// <returns>Returns the command response.</returns>
        [HttpPost("Claims")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ClaimsAsync([FromBody] ClaimsCommand claimsCommand)
        {
            CommandResponse commandResponse = await _mediator.Send(claimsCommand);
            return commandResponse.Success
                ? new OkObjectResult(commandResponse)
                : new BadRequestObjectResult(commandResponse);
        }

        /// <summary>
        /// Verifies the user name uniqueness.
        /// </summary>
        /// <param name="verifyUserNameUniquenessQuery">The verify user name uniqueness query.</param>
        /// <returns>Returns the query response.</returns>
        [AllowAnonymous, HttpPost("VerifyUserNameUniqueness")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerifyUserNameUniquenessAsync([FromBody] VerifyUserNameUniquenessQuery verifyUserNameUniquenessQuery)
        {
            QueryResponse<UserNameUniqueDto> queryResponse = await _mediator.Send(verifyUserNameUniquenessQuery);
            return queryResponse.Success
                ? new OkObjectResult(queryResponse)
                : new BadRequestObjectResult(queryResponse);
        }
    }
}
