namespace Golub.Responses.ProviderResponse
{
    /// <summary>
    /// Record for SendGrid response
    /// </summary>
    public record SendGridResponse : EmailResponse<object>
    {
        public SendGridResponse(bool success, string message) : base(success, message) { }
    }
}
