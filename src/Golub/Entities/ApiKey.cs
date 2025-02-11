using Golub.Common;

namespace Golub.Entities
{
    public class ApiKey : AuditableBaseEntity<Guid>
    {
        public Guid ApiKeyValue { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
        public string ApplicationName { get; set; }
    }
}
