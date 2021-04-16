﻿using Kwetter.Services.Common.Infrastructure.Eventing;
using Kwetter.Services.Common.Infrastructure.Extensions;
using Kwetter.Services.Common.Infrastructure.Integration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Behaviours
{
    /// <summary>
    /// Represents the <see cref="TransactionBehaviour{TRequest,TResponse}"/> class.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public sealed class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse> 
    {
        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
        private readonly IIntegrationEventService _integrationEventService;
        private readonly IAggregateUnitOfWork _unitOfWork;
        private readonly IEventStore _eventStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionBehaviour{TRequest,TResponse}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="integrationEventService">The integration event service.</param>
        /// <param name="unitOfWork">The unit of work associated with the service.</param>
        /// <param name="eventStore">The event store for the service.</param>
        public TransactionBehaviour(
            ILogger<TransactionBehaviour<TRequest, TResponse>> logger,
            IIntegrationEventService integrationEventService,
            IAggregateUnitOfWork unitOfWork,
            IEventStore eventStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        /// <summary>
        /// Ensures that the requests are handled within a transaction.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="next">The request handler delegate.</param>
        /// <returns>Returns the response.</returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response = default;
            string typeName = request.GetGenericTypeName();
            try
            {
                if (_unitOfWork.HasActiveTransaction)
                    return await next();
                IExecutionStrategy strategy = _unitOfWork.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    _logger.LogInformation($"Started transactions for {typeName} {request}");
                    await using IDbContextTransaction transaction = await _unitOfWork.StartTransactionAsync(cancellationToken);
                    await _eventStore.StartTransactionAsync(cancellationToken);
                    response = await next();
                    await _eventStore.CommitTransactionAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync(transaction);
                    _logger.LogInformation($"Commited transactions for {typeName} {request}");
                    await _integrationEventService.PublishEventsThroughEventBusAsync(transaction.TransactionId, cancellationToken);
                });
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR Handling transactions for {typeName} {request}");
                throw;
            }
        }
    }
}
