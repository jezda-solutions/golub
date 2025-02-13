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
        public DbSet<email_provider> EmailProviders { get; set; } = null!;
        public DbSet<sent_email> SentEmails { get; set; } = null!;
        public DbSet<api_key> ApiKeys { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
