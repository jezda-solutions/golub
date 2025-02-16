using Golub.Entities;
using Microsoft.EntityFrameworkCore;

namespace Golub.Contexts
{
    /// <summary>
    /// Application database context class
    /// All DbSets are defined here and configurations are applied
    /// </summary>
    /// <param name="options"></param>
    public class AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : DbContext(options)
    {
        private readonly IConfiguration _configuration
            = configuration ?? throw new ArgumentNullException(nameof(configuration));
        public DbSet<EmailProvider> EmailProviders { get; set; } = null!;
        public DbSet<SentEmail> SentEmails { get; set; } = null!;
        public DbSet<ApiKey> ApiKeys { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            optionsBuilder
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        }
    }
}
