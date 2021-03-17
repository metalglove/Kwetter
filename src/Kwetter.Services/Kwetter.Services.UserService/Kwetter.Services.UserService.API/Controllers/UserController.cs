using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.UserService.API.Application.Commands.CreateUserCommand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kwetter.Services.UserService.API.Controllers
{
    /// <summary>
    /// Represents the <see cref="UserController"/> class.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a user asynchronously through the create user command.
        /// </summary>
        /// <param name="command">The create user command.</param>
        /// <returns>Returns the command response.</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync(CreateUserCommand command)
        {
            CommandResponse commandResponse = await _mediator.Send(command);
            return commandResponse.Success
                ? new CreatedAtRouteResult(new { command.UserId }, commandResponse)
                : BadRequest(commandResponse);
        }
    }
}
