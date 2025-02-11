using Golub.Constants;
using Golub.Entities;
using Golub.Entities.ProviderConfiguration;
using Golub.Requests;
using Golub.Responses;
using Golub.Responses.ProviderResponse;
using Golub.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Text.Json;

namespace Golub.Email.Providers
{
    public class SendGridEmailProvider : IEmailProvider
    {
        public string ProviderName => EmailProviderConstants.SendGrid;

        public async Task<IResponse> SendEmailAsync(SendEmailRequest request, EmailProvider provider)
        {
            var configuration = JsonSerializer.Deserialize<BaseEmailProviderConfiguration>(provider.Configuration);
            var client = new SendGridClient(configuration.ApiKey);
            var from = new EmailAddress(configuration.FromEmail, configuration.FromName);

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

            // Sklonicemo komentar ukoliko nam bude
            // trebao dokaz da slanje emailova radi
            //sendGridMessage.AddBccs(
            //[
            //    new("zoran.jezdimirovic@live.com")
            //]);

            await client.SendEmailAsync(sendGridMessage);

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
