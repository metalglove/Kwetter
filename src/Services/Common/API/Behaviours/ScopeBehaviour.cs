using Kwetter.Services.Common.Infrastructure.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.API.Behaviours
{
    /// <summary>
    /// Represents the <see cref="ScopeBehaviour{TRequest,TResponse}"/> class.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public sealed class ScopeBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<ScopeBehaviour<TRequest, TResponse>> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IServiceScope _scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeBehaviour{TRequest,TResponse}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="serviceScopeFactory">The service scope factory.</param>
        public ScopeBehaviour(ILogger<ScopeBehaviour<TRequest, TResponse>> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _logger.LogInformation($"----- Scope start {typeof(TRequest).GetGenericTypeName()}");
            _scope = _serviceScopeFactory.CreateScope();
        }

        /// <summary>
        /// Creates a scope for the request.
        /// </summary>
        /// <param name="request">The request type.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="next">The request handler delegate.</param>
        /// <returns>Returns a response.</returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response = await next();
            //_scope.Dispose();
            _logger.LogInformation($"----- Scope end {request.GetGenericTypeName()}");
            return response;
        }
    }
}
