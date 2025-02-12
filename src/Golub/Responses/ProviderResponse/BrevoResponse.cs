namespace Golub.Responses.ProviderResponse
{
    public class BrevoResponse(bool success, string message) : IResponse
    {
        public bool Success { get; set; } = success;
        public string Message { get; set; } = message;
    }
}
