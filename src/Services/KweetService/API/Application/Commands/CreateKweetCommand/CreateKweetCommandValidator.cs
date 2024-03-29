﻿using FluentValidation;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.API.Application.Commands.CreateKweetCommand
{
    /// <summary>
    /// Represents the <see cref="CreateKweetCommandValidator"/> class.
    /// </summary>
    public sealed class CreateKweetCommandValidator : AbstractValidator<CreateKweetCommand>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateKweetCommandValidator"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public CreateKweetCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            
            RuleFor(command => command)
                .CustomAsync(KweetValidationAsync);
        }

        private async Task KweetValidationAsync(CreateKweetCommand createKweetCommand, ValidationContext<CreateKweetCommand> context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(createKweetCommand.Message))
            {
                context.AddFailure("The message is null, empty or contains only whitespaces.");
                return;
            }
            if (createKweetCommand.Message.Length > 140)
            {
                context.AddFailure("The length of the message exceeded 140 characters.");
                return;
            }

            // NOTE:
            //  Assumption can be made that the UserId will always be valid since the user
            //  has to be authorized to post a request for creating a kweet.
            if (Guid.Empty == createKweetCommand.UserId)
            {
                context.AddFailure("The user id can not be empty.");
                return;
            }
            if (Guid.Empty == createKweetCommand.KweetId)
            {
                context.AddFailure("The kweet id can not be empty.");
                return;
            }
            Kweet kweet = await _userRepository.FindKweetAsync(createKweetCommand.KweetId, cancellationToken);
            if (kweet != default)
                context.AddFailure("The kweet id already exists.");
        }
    }
}
