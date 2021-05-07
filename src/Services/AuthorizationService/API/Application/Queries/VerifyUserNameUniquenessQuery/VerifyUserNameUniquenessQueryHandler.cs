using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate;
using Kwetter.Services.Common.Application.CQRS;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.API.Application.Queries.VerifyUserNameUniquenessQuery
{
    /// <summary>
    /// Represents the <see cref="VerifyUserNameUniquenessQueryHandler"/> class.
    /// </summary>
    public sealed class VerifyUserNameUniquenessQueryHandler : IRequestHandler<VerifyUserNameUniquenessQuery, QueryResponse<UserNameUniqueDto>>
    {
        private readonly IIdentityRepository _identityRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerifyUserNameUniquenessQueryHandler"/> class.
        /// </summary>
        /// <param name="identityRepository">The identity repository.</param>
        public VerifyUserNameUniquenessQueryHandler(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
        }

        /// <summary>
        /// Handles the verify user name uniqueness query.
        /// </summary>
        /// <param name="request">The verify user name uniqueness query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The query response.</returns>
        public async Task<QueryResponse<UserNameUniqueDto>> Handle(VerifyUserNameUniquenessQuery request, CancellationToken cancellationToken)
        {
            IdentityAggregate identity = await _identityRepository.FindByUserNameAsync(request.UserName, cancellationToken);
            return new QueryResponse<UserNameUniqueDto>()
            {
                Data = new UserNameUniqueDto()
                {
                    IsUnique = identity == default
                },
                Success = true
            };
        }
    }
}
