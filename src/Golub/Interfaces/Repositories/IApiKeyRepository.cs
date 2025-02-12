using Golub.Entities;
using Golub.Repositories;

namespace Golub.Interfaces.Repositories
{
    public interface IApiKeyRepository : IGenericRepository<ApiKey>
    {
        /// <summary>
        /// Gets API Key entity from database by API Key value
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        Task<ApiKey> GetApiKeyAsync(Guid apiKey);
    }
}
