using System;
using Kwetter.Services.Common.API.CQRS;
using MediatR;

namespace Kwetter.Services.FollowService.API.Application.Commands.CreateFollowCommand
{
    /// <summary>
    /// Represents the <see cref="CreateFollowCommand"/> record.
    /// </summary>
    public record CreateFollowCommand : IRequest<CommandResponse>
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