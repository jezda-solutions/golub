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
using System.Text.Json;

namespace Golub.Email.Providers
{
    public class SendGridEmailProvider(IOptions<EmailSettings> emailSettings, ILogger<SendGridEmailProvider> logger) : IEmailProvider
    {
        private readonly EmailSettings _emailSettings = emailSettings.Value;
        private readonly ILogger<SendGridEmailProvider> _logger 
            = logger ?? throw new ArgumentNullException(nameof(logger));

        public string ProviderName => EmailProviderConstants.SendGrid;

        public async Task<IResponse> SendEmailAsync(SendEmailRequest request, EmailProvider provider)
        {
            var configuration = JsonSerializer.Deserialize<BaseEmailProviderConfiguration>(provider.Configuration);

            if (!string.IsNullOrEmpty(request.From))
            {
                if (request.From != configuration.FromEmail)
                {
                    _logger.LogWarning(
                        "The 'From' in the request ({RequestFromEmail}) does not match the configured 'FromEmail' ({ConfiguredFromEmail}). " +
                        "There is a chance that the email might not be sent because the email might not be registered with the provider.",
                        request.From,
                        configuration.FromEmail
                    );
                }
            }

            var client = new SendGridClient(configuration.ApiKey);

            var fromEmail = request.From;
            var fromName = request.FromName;

            if (string.IsNullOrEmpty(fromEmail))
            {
                fromEmail = configuration.FromEmail;
            }

            if (string.IsNullOrEmpty(fromName))
            {
                fromName = configuration.FromName;
            }

            var from = new EmailAddress(fromEmail, fromName);

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
