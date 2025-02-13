using Mandrill.Models;

namespace Golub.Responses.ProviderResponse
{
    public record MandrillResponse : EmailResponse<List<FailedEmailResponse>>
    {
        public MandrillResponse(bool success, string message, List<FailedEmailResponse> failedEmails)
            : base(success, message, failedEmails) { }
    }

    public class FailedEmailResponse
    {
        public EmailResultStatus Status { get; set; }
        public string Email { get; set; }
        public string RejectReason { get; set; }
    }
}
