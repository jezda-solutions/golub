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

namespace Golub.Email.Providers
{
    /// <summary>
    /// Mandrill email provider
    /// Sends emails using Mandrill API
    /// </summary>
    /// <param name="emailSettings"></param>
    public class MandrillEmailProvider(IOptions<EmailSettings> emailSettings) : IEmailProvider
    {
        private readonly EmailSettings _emailSettings = emailSettings.Value;

        public string ProviderName => EmailProviderConstants.Mandrill;

        public async Task<IEmailResponse> SendEmailAsync(SendEmailRequest request, EmailProvider provider, BaseEmailProviderConfiguration configuration)
        {
            var client = new MandrillApi(configuration.ApiKey);

            var tos = new List<EmailAddress>();

            foreach (var email in request.Tos)
            {
                tos.Add(new EmailAddress(email));
            }

            var emailMessage = new EmailMessage()
            {
                To = tos,
                FromEmail = request.From,
                FromName = request.FromName,
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
