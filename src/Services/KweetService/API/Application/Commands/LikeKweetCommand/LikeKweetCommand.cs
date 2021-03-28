using Kwetter.Services.Common.API.CQRS;
using MediatR;
using System;

namespace Kwetter.Services.KweetService.API.Application.Commands.LikeKweetCommand
{
    /// <summary>
    /// Represents the <see cref="LikeKweetCommand"/> record.
    /// </summary>
    public record LikeKweetCommand : IRequest<CommandResponse>
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; init; }
        
        /// <summary>
        /// Gets and sets the kweet id.
        /// </summary>
        public Guid KweetId { get; init; }
    }
}