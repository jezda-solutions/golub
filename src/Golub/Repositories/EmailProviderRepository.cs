using Golub.Contexts;
using Golub.Entities;
using Golub.Interfaces.Repositories;

namespace Golub.Repositories
{
    public class EmailProviderRepository(AppDbContext dbContext)
        : GenericRepository<email_provider>(dbContext), IEmailProviderRepository
    {
    }
}
