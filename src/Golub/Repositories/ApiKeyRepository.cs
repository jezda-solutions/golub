using Golub.Contexts;
using Golub.Entities;
using Golub.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Golub.Repositories
{
    public class ApiKeyRepository(AppDbContext dbContext)
        : GenericRepository<ApiKey>(dbContext), IApiKeyRepository
    {
        private readonly AppDbContext _dbContext 
            = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public async Task<ApiKey> GetApiKeyAsync(Guid apiKey)
        {
            return await _dbContext.ApiKeys.FirstOrDefaultAsync(a => a.ApiKeyValue == apiKey 
                        && (a.ExpirationDate == null || a.ExpirationDate > DateTimeOffset.UtcNow));
        }
    }
}
