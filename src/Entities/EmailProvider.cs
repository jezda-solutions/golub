namespace Golub.Entities
{
    /// <summary>
    /// Represents an email provider in the system
    /// </summary>
    public class EmailProvider
    {
        /// <summary>
        /// Unique identifier of the email provider
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the email provider
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates if the email provider is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Configuration of the email provider in JSON format 
        /// such as API Key, default email address etc.
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// Last time the email provider was used
        /// </summary>
        public DateTimeOffset LastUsedOn { get; set; }

        /// <summary>
        /// Free plan quantity of the email provider for certain period of time
        /// </summary>
        public int? FreePlanQty { get; set; }

        /// <summary>
        /// Remaining quantity of the email provider for free plan
        /// </summary>
        public int? RemainingQty { get; set; }

        /// <summary>
        /// Duration of the free plan in days
        /// After that period, the free plan resets
        /// </summary>
        public int? Period { get; set; }

        /// <summary>
        /// Date and time when the email provider was created
        /// </summary>
        public DateTimeOffset CreatedOnUtc { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Date and time when the email provider was last modified
        /// </summary>
        public DateTimeOffset? ModifiedOnUtc { get; set; }


        /// <summary>
        /// Date and time when the email provider was deleted
        /// </summary>
        public DateTimeOffset? DeletedOnUtc { get; set; }
    }
}
