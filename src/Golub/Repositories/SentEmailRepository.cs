using Golub.Contexts;
using Golub.Entities;
using Golub.Interfaces.Repositories;

namespace Golub.Repositories
{
    public class SentEmailRepository(AppDbContext dbContext)
        : GenericRepository<sent_email>(dbContext), ISentEmailRepository
    {
    }
}
