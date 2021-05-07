using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kwetter.Services.UserService.API.Controllers
{
    /// <summary>
    /// Represents the <see cref="UserController"/> class.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
