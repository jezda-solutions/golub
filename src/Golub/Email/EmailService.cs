using Golub.Requests;
using Golub.Services.Interfaces;

namespace Golub.Email
{
    public class EmailService(IEmailDistributor emailDistributor, ILogger<EmailService> logger) : IEmailService
    {
        private readonly IEmailDistributor _emailDistributor
            = emailDistributor ?? throw new ArgumentNullException(nameof(emailDistributor));
        private readonly ILogger<EmailService> _logger
            = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task SendEmailAsync(SendEmailRequest emailRequest)
        {
            try
            {
                _logger.LogInformation("Starting to process email request for {recipients}", string.Join(", ", emailRequest.Tos));

                await _emailDistributor.DistributeEmailsByCapacityAsync(emailRequest);

                _logger.LogInformation("Successfully sent emails to {recipients}", string.Join(", ", emailRequest.Tos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send emails. Error: {message}", ex.Message);
            }
        }
    }
}
