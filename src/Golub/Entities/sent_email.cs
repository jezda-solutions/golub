namespace Golub.Entities
{
    /// <summary>
    /// Represents a sent email in the system
    /// Catches all emails that tried to be sent and writes them to the database
    /// </summary>
    public class sent_email
    {
        public Guid id { get; set; }
        public Guid email_provider_id { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public bool is_successful { get; set; }
        public string remark { get; set; }
        public DateTimeOffset created_on_utc { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? modified_on_utc { get; set; }

        public virtual email_provider email_provider { get; set; }
    }
}
