using Golub.Common;

namespace Golub.Entities
{
    public class EmailProvider : AuditableBaseEntity<Guid>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Configuration { get; set; }
        public DateTimeOffset LastUsedOn { get; set; }
        public int? FreePlanQty { get; set; }
        public int? RemainingQty { get; set; }
        public int? Period { get; set; }
        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
