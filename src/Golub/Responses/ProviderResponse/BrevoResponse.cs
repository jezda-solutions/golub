namespace Golub.Responses.ProviderResponse
{
    public record BrevoResponse : EmailResponse<object>
    {
        public BrevoResponse(bool success, string message) : base(success, message) { }
    }
}
