using FluentValidation;
using Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.API.Application.Commands.UnlikeKweetCommand
{
    /// <summary>
    /// Represents the <see cref="UnlikeKweetCommandValidator"/> class.
    /// </summary>
    public sealed class UnlikeKweetCommandValidator : AbstractValidator<UnlikeKweetCommand>
    {
        private readonly IKweetRepository _kweetRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnlikeKweetCommandValidator"/> class.
        /// </summary>
        /// <param name="kweetRepository">The kweet repository.</param>
        public UnlikeKweetCommandValidator(IKweetRepository kweetRepository)
        {
            _kweetRepository = kweetRepository ?? throw new ArgumentNullException(nameof(kweetRepository));
            RuleFor(unlikeKweetCommand => unlikeKweetCommand)
                .CustomAsync(UnlikeKweetValidationAsync);
        }

        private async Task UnlikeKweetValidationAsync(UnlikeKweetCommand unlikeKweetCommand, ValidationContext<UnlikeKweetCommand> context, CancellationToken cancellationToken)
        {
            if (unlikeKweetCommand.KweetId == Guid.Empty)
            {
                context.AddFailure("The kweet id can not be empty.");
                return;
            }
            if (unlikeKweetCommand.UserId == Guid.Empty)
            {
                context.AddFailure("The user id can not be empty.");
                return;
            }
            KweetAggregate kweetAggregate = await _kweetRepository.FindAsync(unlikeKweetCommand.KweetId, cancellationToken);
            if (kweetAggregate == default) 
                context.AddFailure("The kweet does not exist.");
        }
    }
}