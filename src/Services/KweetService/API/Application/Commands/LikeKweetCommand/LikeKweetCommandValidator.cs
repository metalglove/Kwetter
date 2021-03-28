using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Threading;
using System.Threading.Tasks;
using Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate;

namespace Kwetter.Services.KweetService.API.Application.Commands.LikeKweetCommand
{
    /// <summary>
    /// Represents the <see cref="LikeKweetCommandValidator"/> class.
    /// </summary>
    public sealed class LikeKweetCommandValidator : AbstractValidator<LikeKweetCommand>
    {
        private readonly IKweetRepository _kweetRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikeKweetCommandValidator"/> class.
        /// </summary>
        /// <param name="kweetRepository">The kweet repository.</param>
        public LikeKweetCommandValidator(IKweetRepository kweetRepository)
        {
            _kweetRepository = kweetRepository;
            
            RuleFor(likeKweetCommand => likeKweetCommand)
                .CustomAsync(LikeKweetValidationAsync);
        }

        private async Task LikeKweetValidationAsync(LikeKweetCommand likeKweetCommand, CustomContext context, CancellationToken cancellationToken)
        {
            if (likeKweetCommand.KweetId == Guid.Empty)
            {
                context.AddFailure("The kweet id can not be empty.");
                return;
            }
            if (likeKweetCommand.UserId == Guid.Empty)
            {
                context.AddFailure("The user id can not be empty.");
                return;
            }
            KweetAggregate kweetAggregate = await _kweetRepository.FindAsync(likeKweetCommand.KweetId, cancellationToken);
            if (kweetAggregate == default) 
                context.AddFailure("The kweet does not exist.");
        }
    }
}