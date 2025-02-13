namespace Golub.Entities
{
    /// <summary>
    /// Represents an email provider in the system
    /// </summary>
    public class email_provider
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public bool is_active { get; set; }

        public string configuration { get; set; }

        public DateTimeOffset last_used_on { get; set; }

        public int? free_plan_qty { get; set; }

        public int? remaining_qty { get; set; }

        public int? period { get; set; }

        public DateTimeOffset created_on_utc { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? modified_on_utc { get; set; }
    }
}
