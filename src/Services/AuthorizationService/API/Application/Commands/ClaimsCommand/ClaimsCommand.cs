using Kwetter.Services.Common.Application.CQRS;
using MediatR;

namespace Kwetter.Services.AuthorizationService.API.Application.Commands.ClaimsCommand
{
    /// <summary>
    /// Represents the <see cref="ClaimsCommand"/> record.
    /// </summary>
    public record ClaimsCommand : IRequest<CommandResponse>
    {
        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets and sets the id token.
        /// </summary>
        public string IdToken { get; set; }
    }
}
