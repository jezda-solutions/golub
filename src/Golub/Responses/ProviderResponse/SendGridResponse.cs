namespace Golub.Responses.ProviderResponse
{
    public class SendGridResponse(bool success, string message) : IResponse
    {
        public bool Success { get; set; } = success;
        public string Message { get; set; } = message;
    }
}
