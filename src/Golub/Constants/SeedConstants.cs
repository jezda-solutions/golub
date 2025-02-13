namespace Golub.Constants
{
    public class SeedConstants
    {
        private static readonly string seedFilesDirectory = "SeedFiles";
        private static readonly string emailProvidersConfigurationPath 
            = Path.Combine(Directory.GetCurrentDirectory(), seedFilesDirectory, "ProviderConfiguration");

        public static string EmailProvidersConfigurationPath => emailProvidersConfigurationPath;
    }
}