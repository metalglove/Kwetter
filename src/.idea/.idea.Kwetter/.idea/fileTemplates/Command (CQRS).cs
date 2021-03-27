using Kwetter.Services.Common.API.CQRS;
using MediatR;
using System;

namespace $NAMESPACE
{
    /// <summary>
    /// Represents the <see cref="${NAME}"/> record.
    /// </summary>
    public record ${NAME} : IRequest<CommandResponse>
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; init; }
    }
}