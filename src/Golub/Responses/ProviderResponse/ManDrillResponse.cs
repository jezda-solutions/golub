using Mandrill.Models;

namespace Golub.Responses.ProviderResponse
{
    /// <summary>
    /// Record for Mandrill response
    /// </summary>
    public record MandrillResponse : EmailResponse<List<FailedEmailResponse>>
    {
        public MandrillResponse(bool success, string message, List<FailedEmailResponse> failedEmails)
            : base(success, message, failedEmails) { }
    }

    /// <summary>
    /// Class for failed emails
    /// Used for Mandrill response
    /// </summary>
    public class FailedEmailResponse
    {
        public EmailResultStatus Status { get; set; }
        public string Email { get; set; }
        public string RejectReason { get; set; }
    }
}
