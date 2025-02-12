namespace Golub.Requests
{
    public class SendEmailRequest
    {
        public IEnumerable<string> Tos { get; set; } = [];

        public IEnumerable<string> Ccs { get; set; } = [];

        public IEnumerable<string> Bcc { get; set; } = [];

        public string Subject { get; set; }

        public string PlainTextContent { get; set; }

        public string From { get; set; }

        public string FromName { get; set; }

        public string InnerHtml { get; set; }
    }
}
