using Kwetter.Services.Common.API.CQRS;
using MediatR;
using System;

namespace Kwetter.Services.UserService.API.Application.Commands.CreateUserCommand
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommand"/> record.
    /// </summary>
    public record CreateUserCommand : IRequest<CommandResponse>
    {
        /// <summary>
        /// Gets or initializes the user id.
        /// </summary>

        public Guid UserId { get; init; }

        /// <summary>
        /// Gets or initializes the user display name.
        /// </summary>
        public string UserDisplayName { get; init; }

        /// <summary>
        /// Gets or initializes the user profile description.
        /// </summary>
        public string UserProfileDescription { get; init; }
    }
}
