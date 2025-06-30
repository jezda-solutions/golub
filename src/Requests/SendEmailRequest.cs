namespace Golub.Requests
{
    /// <summary>
    /// Request for sending email
    /// Use this request to send email
    /// Request contains all necessary information to send email to recipients
    /// </summary>
    public class SendEmailRequest
    {
        public IEnumerable<string> Tos { get; set; } = [];

        public IEnumerable<string> Ccs { get; set; } = [];

        public IEnumerable<string> Bcc { get; set; } = [];

        public string Subject { get; set; }

        public string PlainTextContent { get; set; }

        public string From { get; set; }

        public string FromName { get; set; }

        public required string InnerHtml { get; set; }
    }
}
