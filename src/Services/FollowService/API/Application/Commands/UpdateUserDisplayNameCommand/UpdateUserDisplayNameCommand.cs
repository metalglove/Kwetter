using Kwetter.Services.Common.API.CQRS;
using MediatR;
using System;

namespace Kwetter.Services.FollowService.API.Application.Commands.UpdateUserDisplayNameCommand
{
    /// <summary>
    /// Represents the <see cref="UpdateUserDisplayNameCommand"/> record.
    /// </summary>
    public record UpdateUserDisplayNameCommand : IRequest<CommandResponse>
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Gets and sets the user display name.
        /// </summary>
        public string UserDisplayName { get; init; }
    }
}
