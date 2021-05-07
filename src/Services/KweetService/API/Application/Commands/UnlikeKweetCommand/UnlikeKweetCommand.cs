using Kwetter.Services.Common.Application.CQRS;
using MediatR;
using System;

namespace Kwetter.Services.KweetService.API.Application.Commands.UnlikeKweetCommand
{
    /// <summary>
    /// Represents the <see cref="UnlikeKweetCommand"/> record.
    /// </summary>
    public record UnlikeKweetCommand : IRequest<CommandResponse>
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