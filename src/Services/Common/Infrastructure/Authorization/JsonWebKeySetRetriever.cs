using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Authorization
{
    /// <summary>
    /// Represents the <see cref="JsonWebKeySetRetriever"/> class.
    /// </summary>
    public class JsonWebKeySetRetriever : IConfigurationRetriever<JsonWebKeySet>
    {
        /// <inheritdoc cref="IConfigurationRetriever{T}.GetConfigurationAsync(string, IDocumentRetriever, CancellationToken)" />
        public async Task<JsonWebKeySet> GetConfigurationAsync(string address, IDocumentRetriever retriever, CancellationToken cancel)
        {
            string json = await retriever.GetDocumentAsync(address, cancel);
            return new JsonWebKeySet(json);
        }
    }
}
