
namespace Golub.Entities
{
    /// <summary>
    /// Represents an API key with additional information 
    /// about the key and holder of the key in the system
    /// </summary>
    public class api_key
    {
        public Guid id { get; set; }
        public Guid api_key_value { get; set; }
        public DateTimeOffset? expiration_date { get; set; }
        public string application_name { get; set; }
        public DateTimeOffset created_on_utc { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? modified_on_utc { get; set; }
    }
}
