using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace $NAMESPACE
{
    /// <summary>
    /// Represents the <see cref="${NAME}Validator"/> class.
    /// </summary>
    public sealed class ${NAME}Validator : AbstractValidator<${NAME}>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="${NAME}Validator"/> class.
        /// </summary>
        public ${NAME}Validator()
        {
            RuleFor(${NAME} => ${NAME}.Id)
                .CustomAsync(AsyncValidationExample);
        }

        private Task AsyncValidationExample(Guid id, CustomContext context, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                context.AddFailure("The id can not be empty.");
            }
            return Task.CompletedTask;
        }
    }
}