using Kwetter.Services.AuthorizationService.Infrastructure.Dtos;
using Kwetter.Services.AuthorizationService.Infrastructure.Interfaces;
using Kwetter.Services.Common.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="AuthorizationService"/> class.
    /// Used for authorizating a client using the authorization_code flow.
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthorizationConfiguration _authorizationConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationService"/> class.
        /// </summary>
        /// <param name="options">The authorization configuration options.</param>
        /// <param name="httpClient">The http client instance.</param>
        public AuthorizationService(
            IOptions<AuthorizationConfiguration> options,
            HttpClient httpClient)
        {
            _httpClient = httpClient;
            _authorizationConfiguration = options.Value;
        }

        /// <inheritdoc cref="IAuthorizationService.AuthorizeAsync(string)"/>
        public async Task<AuthorizationDto> AuthorizeAsync(string code)
        {
            HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, _authorizationConfiguration.TokenUri);
            string codeQuery = 
                $"code={code}" +
                $"&client_id={_authorizationConfiguration.ClientId}" +
                $"&client_secret={_authorizationConfiguration.ClientSecret}" +
                $"&redirect_uri=postmessage" +
                $"&grant_type={_authorizationConfiguration.GrantType}";
            HttpContent httpContent = new StringContent(codeQuery, Encoding.UTF8, "application/x-www-form-urlencoded");
            httpRequestMessage.Content = httpContent;
            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            httpResponseMessage.EnsureSuccessStatusCode();
            AuthorizationDto authorizationDto = JsonSerializer.Deserialize<AuthorizationDto>(json);
            return authorizationDto;
        }
    }
}
