﻿using Golub.Interfaces.Repositories;

namespace Golub.Services.ApiKeyServices
{
    /// <summary>
    /// Service for validating API Key
    /// Responsible for checking the validity of the API key
    /// </summary>
    /// <param name="apiKeyRepository"></param>
    public class ApiKeyValidationService(IApiKeyRepository apiKeyRepository)
    {
        private readonly IApiKeyRepository _apiKeyRepository
            = apiKeyRepository ?? throw new ArgumentNullException(nameof(apiKeyRepository));

        /// <summary>
        /// Validates if the API Key is valid and exists in database
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public async Task<bool> IsValidApiKeyAsync(string apiKey, string applicationName)
        {
            if (!Guid.TryParse(apiKey, out var apiKeyGuid)) return false;

            return await _apiKeyRepository.AnyAsync(
                a => a.Id == apiKeyGuid 
                  && a.ApplicationName == applicationName
                  && (a.ExpirationDate == null || a.ExpirationDate > DateTimeOffset.UtcNow)
            );
        }
    }
}
