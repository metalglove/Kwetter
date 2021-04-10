using Kwetter.Services.AuthorizationService.Infrastructure.Dtos;
using Kwetter.Services.Common.API.CQRS;
using MediatR;

namespace Kwetter.Services.AuthorizationService.API.Application.Queries.AuthorizationQuery
{
    /// <summary>
    /// Represents the <see cref="AuthorizationQuery"/> record.
    /// </summary>
    public record AuthorizationQuery : IRequest<QueryResponse<AuthorizationDto>>
    {
        /// <summary>
        /// Gets and sets the authorization code.
        /// </summary>
        public string Code { get; set; }
    }
}
