﻿using System.Threading.Tasks;
using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.KweetService.API.Application.Commands.CreateKweetCommand;
using Kwetter.Services.KweetService.API.Application.Commands.LikeKweetCommand;
using Kwetter.Services.KweetService.API.Application.Commands.UnlikeKweetCommand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kwetter.Services.KweetService.API.Controllers
{
    /// <summary>
    /// Represents the <see cref="KweetController"/> class.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class KweetController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public KweetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a kweet asynchronously through the create kweet command.
        /// </summary>
        /// <param name="command">The create kweet command.</param>
        /// <returns>Returns the command response.</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync(CreateKweetCommand command)
        {
            CommandResponse commandResponse = await _mediator.Send(command);
            return commandResponse.Success
                ? new CreatedAtRouteResult(new {Id = command.KweetId}, commandResponse)
                : BadRequest(commandResponse);
        }
        
        /// <summary>
        /// Likes a kweet asynchronously through the like kweet command.
        /// </summary>
        /// <param name="command">The like kweet command.</param>
        /// <returns>Returns the command response.</returns>
        [HttpPost("Like")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LikeAsync(LikeKweetCommand command)
        {
            CommandResponse commandResponse = await _mediator.Send(command);
            return commandResponse.Success
                ? new OkResult()
                : BadRequest(commandResponse);
        }
        
        /// <summary>
        /// Unlikes a kweet asynchronously through the unlike kweet command.
        /// </summary>
        /// <param name="command">The unlike kweet command.</param>
        /// <returns>Returns the command response.</returns>
        [HttpDelete("Like")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UnlikeAsync(UnlikeKweetCommand command)
        {
            CommandResponse commandResponse = await _mediator.Send(command);
            return commandResponse.Success
                ? new OkResult()
                : BadRequest(commandResponse);
        }
    }
}