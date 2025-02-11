using Golub.Common.Interfaces;
using Golub.Services.Interfaces;

namespace Golub.Common
{
    public abstract class AuditableBaseEntity<T> : AuditableBaseEntity
    {
        public virtual T Id { get; set; }
    }

    public class AuditableBaseEntity : IAuditEntity
    {
        public DateTimeOffset CreatedOnUtc { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? ModifiedOnUtc { get; set; }
    }
}