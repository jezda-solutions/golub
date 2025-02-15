using Golub.Constants;
using Golub.Entities;
using Golub.Entities.ProviderConfiguration;
using Golub.Requests;
using Golub.Responses;
using Golub.Responses.ProviderResponse;
using Golub.Services.Interfaces;
using Golub.Settings;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace Golub.Email.Providers
{
    /// <summary>
    /// SendGrid Email Provider
    /// Sends emails using the SendGrid API
    /// </summary>
    /// <param name="emailSettings"></param>
    public class SendGridEmailProvider(IOptions<EmailSettings> emailSettings) : IEmailProvider
    {
        private readonly EmailSettings _emailSettings = emailSettings.Value;

        public string ProviderName => EmailProviderConstants.SendGrid;

        public async Task<IEmailResponse> SendEmailAsync(SendEmailRequest request, EmailProvider provider, BaseEmailProviderConfiguration configuration)
        {
            var client = new SendGridClient(configuration.ApiKey);

            var from = new EmailAddress(request.From, request.FromName);

            var tos = request.Tos
                .Select(x => new EmailAddress(x))
                .ToList();

            var subjects = tos.Select(email => request.Subject).ToList();
            var substitutions = tos.Select(email => new Dictionary<string, string>()).ToList();

            var sendGridMessage = MailHelper.CreateMultipleEmailsToMultipleRecipients(
                from,
                tos,
                subjects,
                request.PlainTextContent,
                request.InnerHtml,
                substitutions);

            if (request.Bcc != null && request.Bcc.Any())
            {
                var bccs = request.Bcc
                .Select(x => new EmailAddress(x))
                .ToList();

                sendGridMessage.AddBccs(bccs);
            }

            if (!string.IsNullOrEmpty(_emailSettings.Bcc))
            {
                sendGridMessage.AddBccs([new EmailAddress(_emailSettings.Bcc)]);
            }

            try
            {
                var response = await client.SendEmailAsync(sendGridMessage);

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                {
                    return new SendGridResponse(true, "Email sent successfully.");
                }

                return new SendGridResponse(false, $"Failed to send email: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return new SendGridResponse(false, $"Error sending email: {ex.Message}");
            }
        }
    }
}
