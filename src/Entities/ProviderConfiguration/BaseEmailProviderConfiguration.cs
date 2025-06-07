namespace Golub.Entities.ProviderConfiguration
{
    /// <summary>
    /// Base email provider configuration
    /// Responsible for storing email provider specific configuration
    /// Configuration is stored in database as JSON
    /// </summary>
    public class BaseEmailProviderConfiguration
    {
        /// <summary>
        /// API key for email provider gotten from provider 
        /// and stored in configuration of provider in database.
        /// Responsible for authenticating application with email provider.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Email address from which emails are sent 
        /// by default if not specified in request
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// Name of email address from which emails are sent 
        /// by default if not specified in request.
        /// </summary>
        public string FromName { get; set; }
    }
}
