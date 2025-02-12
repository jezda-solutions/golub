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
            var sentEmails = new List<SentEmail>();

            foreach (var emailProvider in _emailProviders)
            {
                if (!Enum.TryParse(emailProvider.ProviderName, out EmailProviderType emailProviderType))
                {
                    throw new Exception($"Za ovog providera '{emailProvider.ProviderName}' nemamo definisan odgovarajući EmailProviderType.");
                }

                var provider
                    = await GetEmailProviderWithCapacityAsync(emailProvider.ProviderName)
                      ?? throw new ArgumentNullException(nameof(emailProvider));

                var remainingCapacity
                    = provider.RemainingQty;

                if (remainingCapacity == null || remainingCapacity == 0) continue;

                var batch
                    = recipients.Take(remainingCapacity.Value);

                if (!batch.Any()) continue;

                try
                {
                    var response = await emailProvider.SendEmailAsync(new SendEmailRequest
                    {
                        Subject = request.Subject,
                        Tos = batch,
                        Ccs = request.Ccs,
                        InnerHtml = request.InnerHtml,
                        From = request.From,
                        FromName = request.FromName,
                        PlainTextContent = request.PlainTextContent
                    }, provider);

                    foreach (var email in batch)
                    {
                        sentEmails.Add(new SentEmail
                        {
                            EmailProviderId = provider.Id,
                            From = request.From,
                            To = email,
                            Subject = request.Subject,
                            IsSuccesful = response.Success,
                            Remark = response.Message,
                        });
                    }

                    await UpdateEmailProviderCapacityAsync(provider, batch.Count());

                    // removing all sent emails, so we don't send them again
                    recipients.RemoveRange(0, batch.Count());

                    _logger.LogInformation("Successfully sent batch of {Count} emails using {ProviderName}.",
                                           batch.Count(),
                                           emailProvider.GetType().Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send batch with {ProviderName}.", emailProvider.GetType().Name);
                }

                if (recipients.Count == 0) break;
            }

            if(sentEmails.Count > 0)
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
        private async Task<EmailProvider> GetEmailProviderWithCapacityAsync(string name)
        {
            var provider = await _emailProviderRepository.GetFirstAsync(x => x.Name == name && x.IsActive);

            if (provider == null) return null;

            if (provider.LastUsedOn.Date != DateTime.Today)
            {
                provider.RemainingQty = provider.FreePlanQty;
            }

            return provider;
        }

        /// <summary>
        /// Updates email provider capacity
        /// </summary>
        /// <param name="emailProvider"></param>
        /// <param name="numberOfEmails"></param>
        /// <returns></returns>
        public async Task UpdateEmailProviderCapacityAsync(EmailProvider emailProvider, int numberOfEmails)
        {
            emailProvider.RemainingQty -= numberOfEmails;
            emailProvider.LastUsedOn = DateTimeOffset.UtcNow;

            _ = await _emailProviderRepository.UpdateAsync(emailProvider);
        }
    }
}
