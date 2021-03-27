using Kwetter.Services.Common.API.CQRS;
using MediatR;
using System;

namespace Kwetter.Services.KweetService.API.Application.Commands.CreateKweetCommand
{
    /// <summary>
    /// Represents the <see cref="CreateKweetCommand"/> record.
    /// </summary>
    public record CreateKweetCommand : IRequest<CommandResponse>
    {
        /// <summary>
        /// Gets and sets the kweet id.
        /// </summary>
        public Guid KweetId { get; init; }
        
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; init; }
        
        /// <summary>
        /// Gets and sets the message.
        /// </summary>
        public string Message { get; init; }
    }
}
