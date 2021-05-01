using Kwetter.Services.AuthorizationService.API.Application.Commands.ClaimsCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous, HttpPost("Claims")]
        public async Task<IActionResult> ClaimsAsync([FromBody] ClaimsCommand claimsCommand)
        {
            return Ok(await _mediator.Send(claimsCommand));
        }
    }
}
