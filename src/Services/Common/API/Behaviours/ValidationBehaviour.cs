using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.API.Behaviours
{
    /// <summary>
    /// Represents the <see cref="ValidationBehaviour{TRequest, TResponse}"/> class.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> _logger;
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly string _validatorName;
        private readonly bool _anyValidators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationBehaviour{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="validators">The validators.</param>
        public ValidationBehaviour(ILogger<ValidationBehaviour<TRequest, TResponse>> logger, IEnumerable<IValidator<TRequest>> validators)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
            _anyValidators = _validators.Any();

            if (_anyValidators)
                _validatorName = _validators.First().GetType().Name;
        }

        /// <summary>
        /// Handles the validations of the requests.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="next">The request handler delegate.</param>
        /// <returns>Returns the response.</returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!_anyValidators)
                return await next();

            _logger.LogInformation($"{_validatorName} started.");
            ValidationContext<TRequest> context = new(request);
            List<ValidationFailure> failures = new();

            foreach (IValidator<TRequest> validator in _validators)
            {
                ValidationResult validationResult = await validator.ValidateAsync(context, cancellationToken);
                if (!validationResult.IsValid)
                    failures.AddRange(validationResult.Errors);
            }

            if (failures.Count != 0)
            {
                _logger.LogWarning($"{_validatorName}: completed with {failures.Count} validation failure{(failures.Count > 1 ? "s" : "")}.");
                throw new ValidationException($"Command Validation Errors for type {typeof(TRequest).Name}", failures);
            }

            _logger.LogInformation($"{_validatorName} completed.");
            return await next();
        }
    }
}
