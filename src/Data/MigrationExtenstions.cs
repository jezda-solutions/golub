using FluentMigrator.Runner;

namespace Golub.Data
{
    /// <summary>
    /// Extensions for FluentMigrator
    /// Adds FluentMigrator to the service collection
    /// Applies migrations
    /// </summary>
    public static class MigrationExtensions
    {
        public static IServiceCollection AddMigrations(this IServiceCollection services, string connectionString)
        {
            services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(MigrationExtensions).Assembly).For.Migrations());

            return services;
        }

        public static void ApplyMigrations(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}
