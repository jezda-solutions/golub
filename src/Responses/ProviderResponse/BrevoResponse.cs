namespace Golub.Responses.ProviderResponse
{
    /// <summary>
    /// Record class for BrevoResponse
    /// </summary>
    public record BrevoResponse : EmailResponse<object>
    {
        public BrevoResponse(bool success, string message) : base(success, message) { }
    }
}
