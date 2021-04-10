using Kwetter.Services.AuthorizationService.Infrastructure.Dtos;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IAuthorizationService"/> interface.
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Performs the authorization using the authorization_code flow.
        /// </summary>
        /// <param name="code">The authorization code.</param>
        /// <returns>Returns the access and refresh token.</returns>
        public Task<AuthorizationDto> AuthorizeAsync(string code);
    }
}
