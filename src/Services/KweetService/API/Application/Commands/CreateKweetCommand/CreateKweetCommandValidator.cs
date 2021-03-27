using FluentValidation;
using FluentValidation.Validators;

namespace Kwetter.Services.KweetService.API.Application.Commands.CreateKweetCommand
{
    /// <summary>
    /// Represents the <see cref="CreateKweetCommandValidator"/> class.
    /// </summary>
    public sealed class CreateKweetCommandValidator : AbstractValidator<CreateKweetCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateKweetCommandValidator"/> class.
        /// </summary>
        public CreateKweetCommandValidator()
        {
            // NOTE:
            //  Assumption can be made that the UserId will always be valid since the user
            //  has to be authorized to post a request for creating a kweet.
            // command.UserId
            
            RuleFor(command => command.Id)
                .NotEmpty()
                .WithMessage("The kweet id can not be empty.");
            
            RuleFor(command => command.Message)
                .Custom(KweetMessageValidation);
        }

        private static void KweetMessageValidation(string message, CustomContext context)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                context.AddFailure("The message is null, empty or contains only whitespaces.");
                return;
            }
            if (message.Length > 140)
            {
                context.AddFailure("The length of the message exceeded 140 characters.");
            }
        }
    }
}
