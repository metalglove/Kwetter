using Kwetter.Services.Common.API.CQRS;
using MediatR;
using System;

namespace Kwetter.Services.FollowService.API.Application.Commands.UpdateUserProfilePictureUrlCommand
{
    /// <summary>
    /// Represents the <see cref="UpdateUserProfilePictureUrlCommand"/> record.
    /// </summary>
    public record UpdateUserProfilePictureUrlCommand : IRequest<CommandResponse>
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Gets and sets the user profile picture url.
        /// </summary>
        public string UserProfilePictureUrl { get; init; }
    }
}
