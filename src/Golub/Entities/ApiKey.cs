
namespace Golub.Entities
{
    public class ApiKey
    {
        public Guid Id { get; set; }
        public Guid ApiKeyValue { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
        public string ApplicationName { get; set; }
        public DateTimeOffset CreatedOnUtc { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedOnUtc { get; set; }
    }
}
