namespace Golub.Responses.HandlerResponse.EmailProviderResponse
{
    public class GetEmailProviderByIdQueryResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string Configuration { get; set; }

        public int? FreePlanQty { get; set; }

        public int? RemainingQty { get; set; }

        public int? Period { get; set; }
    }
}
