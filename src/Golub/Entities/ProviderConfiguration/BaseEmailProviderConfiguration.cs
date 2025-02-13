namespace Golub.Entities.ProviderConfiguration
{
    /// <summary>
    /// Base email provider configuration
    /// Responsible for storing email provider specific configuration
    /// Configuration is stored in database as JSON
    /// </summary>
    public class BaseEmailProviderConfiguration
    {
        public string ApiKey { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }
}
