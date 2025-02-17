namespace Golub.Responses.HandlerResponse.EmailProviderResponse
{
    public class GetAllEmailProvidersResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool HasConfiguration { get; set; }

        public int? FreePlanQty { get; set; }

        public int? RemainingQty { get; set; }

        public int? Period { get; set; }
    }
}
