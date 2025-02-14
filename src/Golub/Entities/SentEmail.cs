namespace Golub.Entities
{
    /// <summary>
    /// Represents a sent email in the system
    /// Catches all emails that tried to be sent and writes them to the database
    /// </summary>
    public class SentEmail
    {
        public Guid Id { get; set; }
        public Guid EmailProviderId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public bool IsSuccessful { get; set; }
        public string Remark { get; set; }
        public DateTimeOffset CreatedOnUtc { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedOnUtc { get; set; }

        public virtual EmailProvider EmailProvider { get; set; }
    }
}
