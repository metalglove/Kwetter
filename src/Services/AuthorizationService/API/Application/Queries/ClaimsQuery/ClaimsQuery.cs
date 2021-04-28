using Kwetter.Services.AuthorizationService.Infrastructure.Dtos;
using Kwetter.Services.Common.API.CQRS;
using MediatR;

namespace Kwetter.Services.AuthorizationService.API.Application.Queries.ClaimsQuery
{
    /// <summary>
    /// Represents the <see cref="ClaimsQuery"/> record.
    /// </summary>
    public record ClaimsQuery : IRequest<QueryResponse<ClaimsDto>>
    {
        /// <summary>
        /// Gets and sets the id token.
        /// </summary>
        public string IdToken { get; set; }
    }
}
