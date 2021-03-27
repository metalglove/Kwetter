using Kwetter.Services.Common.API.CQRS;
using MediatR;
using System;

namespace Kwetter.Services.FollowService.API.Application.Commands.DeleteFollowCommand
{
    /// <summary>
    /// Represents the <see cref="DeleteFollowCommand"/> record.
    /// </summary>
    public record DeleteFollowCommand : IRequest<CommandResponse>
    {
        /// <summary>
        /// Gets and sets the following id.
        /// </summary>
        public Guid FollowingId { get; init; }

        /// <summary>
        /// Gets and sets the follower id.
        /// </summary>
        public Guid FollowerId { get; init; }
    }
}