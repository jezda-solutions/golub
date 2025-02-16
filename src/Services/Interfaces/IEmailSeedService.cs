namespace Golub.Services.Interfaces
{
    public interface IEmailSeedService
    {
        /// <summary>
        /// Seeds the database with email providers
        /// that are stored in the configuration files (.json files)
        /// </summary>
        /// <returns></returns>
        Task SeedAsync();
    }
}