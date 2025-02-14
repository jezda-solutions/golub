namespace Golub.Entities
{
    /// <summary>
    /// Represents a sent email in the system
    /// Catches all emails that tried to be sent and writes them to the database
    /// </summary>
    public class SentEmail
    {
        /// <summary>
        /// Unique identifier of the sent email
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Unique identifier of the email provider that sent the email
        /// </summary>
        public Guid EmailProviderId { get; set; }

        /// <summary>
        /// Email address of the sender
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Email address of the recipient
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Subject of the email
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Property that indicates if the email was sent successfully
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Any additional information about the email such as error message
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Date and time when the entity was created
        /// </summary>
        public DateTimeOffset CreatedOnUtc { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Date and time when the entity was last modified
        /// </summary>
        public DateTimeOffset? ModifiedOnUtc { get; set; }

        /// <summary>
        /// Navigation property to the email provider that sent the email
        /// </summary>
        public virtual EmailProvider EmailProvider { get; set; }
    }
}
