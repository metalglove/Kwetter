using Kwetter.Services.Common.Application.CQRS;
using MediatR;

namespace Kwetter.Services.AuthorizationService.API.Application.Queries.VerifyUserNameUniquenessQuery
{
    /// <summary>
    /// Represents the <see cref="VerifyUserNameUniquenessQuery"/> record.
    /// </summary>
    public record VerifyUserNameUniquenessQuery : IRequest<QueryResponse<UserNameUniqueDto>>
    {
        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string UserName { get; set; }
    }
}
