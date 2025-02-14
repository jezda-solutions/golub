using brevo_csharp.Api;
using brevo_csharp.Client;
using brevo_csharp.Model;
using Golub.Constants;
using Golub.Entities;
using Golub.Entities.ProviderConfiguration;
using Golub.Requests;
using Golub.Responses;
using Golub.Responses.ProviderResponse;
using Golub.Services.Interfaces;
using Golub.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Golub.Email.Providers
{
    /// <summary>
    /// Brevo Email Provider
    /// Sends emails using Brevo API
    /// </summary>
    /// <param name="emailSettings"></param>
    /// <param name="logger"></param>
    public class BrevoEmailProvider(IOptions<EmailSettings> emailSettings, ILogger<BrevoEmailProvider> logger) : IEmailProvider
    {
        private readonly EmailSettings _emailSettings = emailSettings.Value;
        private readonly ILogger<BrevoEmailProvider> _logger
            = logger ?? throw new ArgumentNullException(nameof(logger));

        public string ProviderName => EmailProviderConstants.Brevo;

        public async Task<IEmailResponse> SendEmailAsync(SendEmailRequest request, EmailProvider provider, BaseEmailProviderConfiguration configuration)
        {
            Configuration.Default.AddApiKey("api-key", configuration.ApiKey);

            var apiInstance = new TransactionalEmailsApi();

            var sender = new SendSmtpEmailSender(request.FromName, request.From);

            var tos = request.Tos
               .Select(x => new SendSmtpEmailTo(x, x))
               .ToList();

            var sendSmtpEmail = new SendSmtpEmail
            {
                Sender = sender,
                To = tos,
                Subject = request.Subject,
                HtmlContent = request.InnerHtml,
                TextContent = request.PlainTextContent
            };

            if (!string.IsNullOrEmpty(_emailSettings.Bcc))
            {
                sendSmtpEmail.Bcc =
                [
                    new(_emailSettings.Bcc)
                ];
            }

            if (request.Bcc != null && request.Bcc.Any())
            {
                var bccs = request.Bcc.Select(x => new SendSmtpEmailBcc(x, x));

                sendSmtpEmail.Bcc.AddRange(bccs);
            }

            try
            {
                var response = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);

                if (response.MessageId != null)
                {
                    return new BrevoResponse(true, "Email sent successfully.");
                }

                return new BrevoResponse(false, "Failed to send email.");
            }
            catch (Exception ex)
            {
                return new BrevoResponse(false, $"Error sending email: {ex.Message}");
            }
        }
    }
}
