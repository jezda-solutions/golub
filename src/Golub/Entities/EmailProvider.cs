namespace Golub.Entities
{
    public class EmailProvider
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Configuration { get; set; }
        public DateTimeOffset LastUsedOn { get; set; }
        public int? FreePlanQty { get; set; }
        public int? RemainingQty { get; set; }
        public int? Period { get; set; }
        public DateTimeOffset CreatedOnUtc { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedOnUtc { get; set; }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
