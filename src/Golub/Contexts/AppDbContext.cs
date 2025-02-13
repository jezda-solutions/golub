using Golub.Entities;
using Microsoft.EntityFrameworkCore;

namespace Golub.Contexts
{
    /// <summary>
    /// Application database context class
    /// All DbSets are defined here and configurations are applied
    /// </summary>
    /// <param name="options"></param>
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<EmailProvider> EmailProviders { get; set; } = null!;
        public DbSet<SentEmail> SentEmails { get; set; } = null!;
        public DbSet<ApiKey> ApiKeys { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
