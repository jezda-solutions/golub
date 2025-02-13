using Golub.Constants;
using Golub.Entities;
using Golub.Entities.ProviderConfiguration;
using Golub.Requests;
using Golub.Responses;
using Golub.Responses.ProviderResponse;
using Golub.Services.Interfaces;
using Golub.Settings;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Golub.Email.Providers
{
    public class MandrillEmailProvider(IOptions<EmailSettings> emailSettings, ILogger<MandrillEmailProvider> logger) : IEmailProvider
    {
        private readonly EmailSettings _emailSettings = emailSettings.Value;
        private readonly ILogger<MandrillEmailProvider> _logger
            = logger ?? throw new ArgumentNullException(nameof(logger));

        public string ProviderName => EmailProviderConstants.Mandrill;

        public async Task<IEmailResponse> SendEmailAsync(SendEmailRequest request, EmailProvider provider)
        {
            var configuration = JsonSerializer.Deserialize<BaseEmailProviderConfiguration>(provider.Configuration);

            if (!string.IsNullOrEmpty(request.From) && request.From != configuration.FromEmail)
            {
                _logger.LogWarning(
                        "The 'FromEmail' in the request ({RequestFromEmail}) does not match the configured 'FromEmail' ({ConfiguredFromEmail}). " +
                        "There is a chance that the email might not be sent because the email might not be registered with the provider.",
                        request.From,
                        configuration.FromEmail
                );
            }

            var client = new MandrillApi(configuration.ApiKey);

            var tos = new List<EmailAddress>();

            foreach (var email in request.Tos)
            {
                tos.Add(new EmailAddress(email));
            }

            var fromEmail = request.From ?? configuration.FromEmail;
            var fromName = request.FromName ?? configuration.FromName;

            var emailMessage = new EmailMessage()
            {
                To = tos,
                FromEmail = fromEmail,
                FromName = fromName,
                Subject = request.Subject,
                Text = request.PlainTextContent
            };

            if (!string.IsNullOrEmpty(_emailSettings.Bcc))
            {
                emailMessage.BccAddress = _emailSettings.Bcc;
            }

            var emailRequest = new SendMessageRequest(emailMessage);

            try
            {
                var responses = await client.SendMessage(emailRequest);

                var mandrillResponse = new MandrillResponse(true, "Email sent successfully", []);

                foreach (var response in responses)
                {
                    if (response.Status != EmailResultStatus.Sent)
                    {
                        mandrillResponse = mandrillResponse with
                        {
                            Data =
                            [
                                .. mandrillResponse.Data,
                                new FailedEmailResponse
                                    {
                                        Status = response.Status,
                                        Email = response.Email,
                                        RejectReason = response.RejectReason
                                    },
                            ]
                        };
                    }
                }

                return mandrillResponse;
            }
            catch (Exception ex)
            {
                return new MandrillResponse(false, $"Error sending email: {ex.Message}", []);
            }
        }
    }
}
