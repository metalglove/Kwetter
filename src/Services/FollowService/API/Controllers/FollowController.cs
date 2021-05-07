using Kwetter.Services.Common.Application.CQRS;
using Kwetter.Services.FollowService.API.Application.Commands.CreateFollowCommand;
using Kwetter.Services.FollowService.API.Application.Commands.DeleteFollowCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.Services.FollowService.API.Controllers
{
    /// <summary>
    /// Represents the <see cref="FollowController"/> class.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FollowController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FollowController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public FollowController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// Creates a follow asynchronously through the create follow command.
        /// </summary>
        /// <param name="command">The create follow command.</param>
        /// <returns>Returns the command response.</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync(CreateFollowCommand command)
        {
            Guid userId = Guid.Parse(HttpContext.User.Claims.Single(claim => claim.Type == "UserId").Value);
            if (command.FollowerId != userId)
                return UnauthorizedCommand();

            CommandResponse commandResponse = await _mediator.Send(command);
            return commandResponse.Success
                ? new CreatedAtRouteResult(new {command.FollowingId, command.FollowerId}, commandResponse)
                : BadRequest(commandResponse);
        }

        /// <summary>
        /// Deletes a follow asynchronously through the delete follow command.
        /// </summary>
        /// <param name="command">The delete follow command.</param>
        /// <returns>Returns the command response.</returns>
        [HttpDelete("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(DeleteFollowCommand command)
        {
            Guid userId = Guid.Parse(HttpContext.User.Claims.Single(claim => claim.Type == "UserId").Value);
            if (command.FollowerId != userId)
                return UnauthorizedCommand();

            CommandResponse commandResponse = await _mediator.Send(command);
            return commandResponse.Success
                ? new OkObjectResult(commandResponse)
                : BadRequest(commandResponse);
        }

        private IActionResult UnauthorizedCommand()
        {
            return Unauthorized(new CommandResponse()
            {
                Errors = new List<string>() { "The user id and follower id are not the same." },
                Success = false
            });
        }
    }
}
