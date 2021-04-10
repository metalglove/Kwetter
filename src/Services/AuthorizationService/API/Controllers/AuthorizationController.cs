using Kwetter.Services.AuthorizationService.API.Application.Queries.AuthorizationQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorizationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous, HttpPost("Code")]
        public async Task<IActionResult> CallbackAsync([FromBody] AuthorizationQuery authorizationQuery)
        {
            return Ok(await _mediator.Send(authorizationQuery));
        }
    }
}
