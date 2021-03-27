using System.Threading.Tasks;
using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.FollowService.API.Application.Commands.CreateFollowCommand;
using Kwetter.Services.FollowService.API.Application.Commands.DeleteFollowCommand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kwetter.Services.FollowService.API.Controllers
{
    /// <summary>
    /// Represents the <see cref="FollowController"/> class.
    /// </summary>
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
            CommandResponse commandResponse = await _mediator.Send(command);
            return commandResponse.Success
                ? new OkResult()
                : BadRequest(commandResponse);
        }
    }
}
