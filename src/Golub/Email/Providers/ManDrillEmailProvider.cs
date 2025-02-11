using Golub.Constants;
using Golub.Entities;
using Golub.Entities.ProviderConfiguration;
using Golub.Requests;
using Golub.Responses;
using Golub.Responses.ProviderResponse;
using Golub.Services.Interfaces;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using System.Text.Json;

namespace Golub.Email.Providers
{
    public class ManDrillEmailProvider : IEmailProvider
    {
        public string ProviderName => EmailProviderConstants.ManDrill;

        public async Task<IResponse> SendEmailAsync(SendEmailRequest request, EmailProvider provider)
        {
            var configuration = JsonSerializer.Deserialize<BaseEmailProviderConfiguration>(provider.Configuration);
            var client = new MandrillApi(configuration.ApiKey);

            var tos = new List<EmailAddress>();

            foreach (var email in request.Tos)
            {
                tos.Add(new EmailAddress(email));
            }

            var emailMessage = new EmailMessage()
            {
                To = tos,
                FromEmail = configuration.FromEmail,
                FromName = configuration.FromName,
                Subject = request.Subject,
                Text = request.PlainTextContent,
            };

            var emailRequest = new SendMessageRequest(emailMessage);

            try
            {
                var response = await client.SendMessage(emailRequest);

                var manDrillResponse = new ManDrillResponse(true, "Email sent successfully");

                foreach (var result in response)
                {
                    if (result.Status != EmailResultStatus.Sent)
                    {
                        manDrillResponse.Success = false;
                    }
                }

                if (!manDrillResponse.Success)
                {
                    manDrillResponse.Message = "Some emails failed to send.";
                }

                return manDrillResponse;
            }
            catch (Exception ex)
            {
                return new ManDrillResponse(false, $"Error sending email: {ex.Message}");
            }
        }
    }
}
