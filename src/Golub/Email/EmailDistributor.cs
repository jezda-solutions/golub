using Golub.Entities;
using Golub.Enums;
using Golub.Interfaces.Repositories;
using Golub.Requests;
using Golub.Services.Interfaces;

namespace Golub.Email
{
    public class EmailDistributor(IEnumerable<IEmailProvider> emailProviders,
                                  ILogger<EmailDistributor> logger,
                                  IEmailProviderRepository emailProviderRepository,
                                  ISentEmailRepository sentEmailRepository) : IEmailDistributor
    {
        private readonly IEnumerable<IEmailProvider> _emailProviders
            = emailProviders ?? throw new ArgumentNullException(nameof(emailProviders));
        private readonly ILogger<EmailDistributor> _logger
            = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IEmailProviderRepository _emailProviderRepository
            = emailProviderRepository ?? throw new ArgumentNullException(nameof(emailProviderRepository));
        private readonly ISentEmailRepository _sentEmailRepository
            = sentEmailRepository ?? throw new ArgumentNullException(nameof(sentEmailRepository));

        public async Task DistributeEmailsByCapacityAsync(SendEmailRequest request)
        {
            var recipients = request.Tos.ToList();
            var sentEmails = new List<sent_email>();

            foreach (var emailProvider in _emailProviders)
            {
                if (!Enum.TryParse(emailProvider.ProviderName, out EmailProviderType emailProviderType))
                {
                    _logger.LogError("Email provider '{ProviderName}' not found.", emailProvider.ProviderName);
                    continue;
                }

                var provider = await GetEmailProviderWithCapacityAsync(emailProvider.ProviderName);

                if (provider == null)
                {
                    _logger.LogError("Email provider '{ProviderName}' not found.", emailProvider.ProviderName);
                    continue;
                }

                var remainingCapacity
                    = provider.remaining_qty;

                if (remainingCapacity == null || remainingCapacity == 0) continue;

                var emailsToSend = recipients.Take(remainingCapacity.Value);

                if (!emailsToSend.Any()) continue;

                try
                {
                    var response = await emailProvider.SendEmailAsync(new SendEmailRequest
                    {
                        Subject = request.Subject,
                        Tos = emailsToSend,
                        Ccs = request.Ccs,
                        InnerHtml = request.InnerHtml,
                        From = request.From,
                        FromName = request.FromName,
                        PlainTextContent = request.PlainTextContent
                    }, provider);

                    foreach (var email in emailsToSend)
                    {
                        sentEmails.Add(new sent_email
                        {
                            email_provider_id = provider.id,
                            from = request.From,
                            to = email,
                            subject = request.Subject,
                            is_successful = response.Success,
                            remark = response.Message,
                        });
                    }

                    await UpdateEmailProviderCapacityAsync(provider, emailsToSend.Count());

                    // removing all sent emails, so we don't send them again
                    recipients.RemoveRange(0, emailsToSend.Count());

                    _logger.LogInformation("Successfully sent batch of {Count} emails using {ProviderName}.",
                                           emailsToSend.Count(),
                                           emailProvider.GetType().Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send batch with {ProviderName}.", emailProvider.GetType().Name);
                }

                if (recipients.Count == 0) break;
            }

            if (sentEmails.Count > 0)
            {
                await _sentEmailRepository.AddRangeAsync(sentEmails);
            }

            if (recipients.Count > 0)
            {
                _logger.LogError("Not enough capacity to send all emails. Remaining: {Count}",
                                 request.Tos.Count());
            }
        }

        /// <summary>
        /// Returns email provider with remaining capacity
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task<email_provider> GetEmailProviderWithCapacityAsync(string name)
        {
            var provider = await _emailProviderRepository.GetFirstAsync(x => x.name == name && x.is_active);

            if (provider == null) return null;

            if (provider.last_used_on.Date != DateTime.Today)
            {
                provider.remaining_qty = provider.free_plan_qty;
            }

            return provider;
        }

        /// <summary>
        /// Updates email provider capacity
        /// </summary>
        /// <param name="emailProvider"></param>
        /// <param name="numberOfEmails"></param>
        /// <returns></returns>
        public async Task UpdateEmailProviderCapacityAsync(email_provider emailProvider, int numberOfEmails)
        {
            emailProvider.remaining_qty -= numberOfEmails;
            emailProvider.last_used_on = DateTimeOffset.UtcNow;

            await _emailProviderRepository.UpdateAsync(emailProvider);
        }
    }
}
