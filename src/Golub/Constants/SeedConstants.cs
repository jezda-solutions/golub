namespace Golub.Constants
{
    public class SeedConstants
    {
        private static readonly string SeedFilesDirectory = "SeedFiles";
        private static readonly string emailProvidersConfigurationPath 
            = Path.Combine(Directory.GetCurrentDirectory(), SeedFilesDirectory, "ProviderConfiguration");

        public static string EmailProvidersConfigurationPath => emailProvidersConfigurationPath;
    }
}