namespace Golub.Responses.ProviderResponse
{
    public record SendGridResponse : EmailResponse<object>
    {
        public SendGridResponse(bool success, string message) : base(success, message) { }
    }
}
