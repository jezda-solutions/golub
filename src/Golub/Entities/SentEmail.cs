using Golub.Common;

namespace Golub.Entities
{
    public class SentEmail : AuditableBaseEntity<Guid>
    {
        public Guid EmailProviderId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public bool IsSuccesful { get; set; }
        public string Remark { get; set; }

        public virtual EmailProvider EmailProvider { get; set; }
    }
}
