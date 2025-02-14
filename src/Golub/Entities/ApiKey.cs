
namespace Golub.Entities
{
    /// <summary>
    /// Represents an API key with additional information 
    /// about the key and holder of the key in the system
    /// </summary>
    public class ApiKey
    {
        /// <summary>
        /// The unique identifier of the API key
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The unique value of the API key
        /// </summary>
        public Guid ApiKeyValue { get; set; }

        /// <summary>
        /// Expiration date of the API key that can be nullable
        /// </summary>
        public DateTimeOffset? ExpirationDate { get; set; }

        /// <summary>
        /// Name of the application that holds the API key
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Date and time when the API key entity was created
        /// </summary>
        public DateTimeOffset CreatedOnUtc { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Date and time when the API key entity was last modified
        /// </summary>
        public DateTimeOffset? ModifiedOnUtc { get; set; }
    }
}
