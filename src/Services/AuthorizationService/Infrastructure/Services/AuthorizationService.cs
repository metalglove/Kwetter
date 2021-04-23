using Kwetter.Services.AuthorizationService.Infrastructure.Dtos;
using Kwetter.Services.AuthorizationService.Infrastructure.Interfaces;
using Kwetter.Services.Common.Application.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly AuthorizationConfiguration _authorizationConfiguration;
        private readonly ILogger<AuthorizationService> _logger;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationService"/> class.
        /// </summary>
        /// <param name="options">The authorization configuration options.</param>
        /// <param name="httpClient">The http client instance.</param>
        /// <param name="logger">The logger.</param>
        public AuthorizationService(
            IOptions<AuthorizationConfiguration> options,
            HttpClient httpClient,
            ILogger<AuthorizationService> logger)
        {
            _authorizationConfiguration = options.Value;
            _logger = logger;
            _httpClient = httpClient;
        }

        /// <inheritdoc cref="IAuthorizationService.AuthorizeAsync(string)"/>
        public async Task<AuthorizationDto> AuthorizeAsync(string code)
        {
            try
            {
                HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, _authorizationConfiguration.TokenUri);
                string codeQuery =
                    $"code={code}" +
                    $"&client_id={_authorizationConfiguration.ClientId}" +
                    $"&client_secret={_authorizationConfiguration.ClientSecret}" +
                    $"&redirect_uri=postmessage" +
                    $"&grant_type=authorization_code";
                _logger.LogInformation($"codeQuery: {codeQuery}");
                HttpContent httpContent = new StringContent(codeQuery, Encoding.UTF8, "application/x-www-form-urlencoded");
                httpRequestMessage.Content = httpContent;
                _logger.LogInformation("About to send request to google!");
                HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);
                _logger.LogInformation("Received response from google!");
                _logger.LogInformation("Reading json from response 1....");
                using (StreamReader reader = new(await httpResponseMessage.Content.ReadAsStreamAsync()))
                {
                    string response = reader.ReadToEnd();
                    _logger.LogInformation(response);
                }

                _logger.LogInformation("Reading json from response 2....");
                string json = await httpResponseMessage.Content.ReadAsStringAsync();

                StringBuilder builder = new(Environment.NewLine);
                builder.AppendLine($"JSON:{Environment.NewLine}{json}");
                builder.AppendLine($"HTTP StatusCode:{Environment.NewLine}{httpResponseMessage.StatusCode}");
                builder.AppendLine($"HTTP ReasonPhrase:{Environment.NewLine}{httpResponseMessage.ReasonPhrase}");
                builder.AppendLine($"HTTP Response headers:");
                foreach (KeyValuePair<string, IEnumerable<string>> header in httpResponseMessage.Headers)
                    builder.AppendLine($"\t{header.Key}:{string.Join(',', header.Value)}");
                _logger.LogInformation(builder.ToString());

                httpResponseMessage.EnsureSuccessStatusCode();
                AuthorizationDto authorizationDto = JsonSerializer.Deserialize<AuthorizationDto>(json);
                return authorizationDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
