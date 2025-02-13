using Golub.Contexts;
using Golub.Entities;
using Golub.Interfaces.Repositories;

namespace Golub.Repositories
{
    public class ApiKeyRepository(AppDbContext dbContext)
        : GenericRepository<api_key>(dbContext), IApiKeyRepository
    {
    }
}
