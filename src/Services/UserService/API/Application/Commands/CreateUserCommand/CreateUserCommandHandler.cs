using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.Common.Infrastructure.Integration;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.UserService.API.Application.Commands.CreateUserCommand
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommandHandler"/> class.
    /// </summary>
    public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CommandResponse>
    {
        private readonly IIntegrationEventService _integrationEventService;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
        /// </summary>
        /// <param name="integrationEventService">The integration event service.</param>
        /// <param name="userRepository">The user repository.</param>
        public CreateUserCommandHandler(
            IIntegrationEventService integrationEventService,
            IUserRepository userRepository
            )
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Handles the <see cref="CreateUserCommand"/>.
        /// </summary>
        /// <param name="request">The create user command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // TODO: UserCreationStartedIntegrationEvent?
            // NOTE:
            // Integration Events notes: 
            // An Event is “something that has happened in the past”, therefore its name has to be   
            // An Integration Event is an event that can cause side effects to other microservices, Bounded-Contexts or external systems.
            // --------------------------
            // UserCreationStartedIntegrationEvent userCreationStartedIntegrationEvent = new UserCreationStartedIntegrationEvent();
            // await _integrationEventService.AddAndSaveEventAsync(event, cancellationToken);

            // Only the aggregate root can be created.
            // The user profile will be created by the aggregate root.
            UserAggregate userAggregate = new(request.UserId, request.UserDisplayName, request.UserProfileDescription);

            // Creates the user aggregate.
            _userRepository.Create(userAggregate);

            // Saves the created entities.
            bool success = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            // Prepares the command response.
            CommandResponse commandResponse = new()
            {
                Success = success
            };

            return commandResponse;
        }
    }
}
