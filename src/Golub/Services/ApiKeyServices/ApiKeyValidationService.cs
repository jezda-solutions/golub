using Golub.Interfaces.Repositories;

namespace Golub.Services.ApiKeyServices
{
    public class ApiKeyValidationService(IApiKeyRepository apiKeyRepository)
    {
        private readonly IApiKeyRepository _apiKeyRepository 
            = apiKeyRepository ?? throw new ArgumentNullException(nameof(apiKeyRepository));

        /// <summary>
        /// Validates if the API Key is valid and exists in database
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public async Task<bool> IsValidApiKeyAsync(string apiKey)
        {
            if (!Guid.TryParse(apiKey, out var apiKeyGuid)) return false;

            return await _apiKeyRepository.AnyAsync(a => a.ApiKeyValue == apiKeyGuid
                                                         && (a.ExpirationDate == null || a.ExpirationDate > DateTimeOffset.UtcNow));
        }
    }
}
