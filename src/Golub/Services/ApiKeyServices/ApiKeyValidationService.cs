using Golub.Interfaces.Repositories;

namespace Golub.Services.ApiKeyServices
{
    public class ApiKeyValidationService(IApiKeyRepository apiKeyRepository)
    {
        private readonly IApiKeyRepository _apiKeyRepository 
            = apiKeyRepository ?? throw new ArgumentNullException(nameof(apiKeyRepository));

        public async Task<bool> IsValidApiKeyAsync(string apiKey)
        {
            if (!Guid.TryParse(apiKey, out var apiKeyGuid)) return false;

            var apiKeyEntity = await _apiKeyRepository.GetApiKeyAsync(apiKeyGuid);
            return apiKeyEntity != null;
        }
    }
}
