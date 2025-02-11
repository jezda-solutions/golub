using Golub.Entities;
using Golub.Repositories;

namespace Golub.Interfaces.Repositories
{
    public interface IApiKeyRepository : IGenericRepository<ApiKey>
    {
        Task<ApiKey> GetApiKeyAsync(Guid apiKey);
    }
}
