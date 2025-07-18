﻿using Golub.Entities;
using Golub.Entities.ProviderConfiguration;
using Golub.Enums;
using Golub.Interfaces.Repositories;
using Golub.Requests;
using Golub.Services.Interfaces;
using System.Text.Json;

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

            var emailProviders = _emailProviders.OrderBy(x => x.Priority);

            foreach (var emailProvider in emailProviders)
            {
                if (!Enum.TryParse(emailProvider.ProviderName, out EmailProviderType emailProviderType))
                {
                    _logger.LogWarning("Email provider '{ProviderName}' not found.", emailProvider.ProviderName);
                    continue;
                }

                var provider = await GetEmailProviderWithCapacityAsync(emailProvider.ProviderName);

                if (provider == null)
                {
                    _logger.LogWarning("Email provider '{ProviderName}' not found.", emailProvider.ProviderName);
                    continue;
                }

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

                var remainingCapacity
                    = provider.RemainingQty;

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
                        From = !string.IsNullOrEmpty(request.From) 
                             ? request.From 
                             : configuration.FromEmail,
                        FromName = request.FromName ?? configuration.FromName,
                        PlainTextContent = request.PlainTextContent,
                        Bcc = request.Bcc
                    }, provider, configuration);

                    foreach (var email in emailsToSend)
                    {
                        sentEmails.Add(new SentEmail
                        {
                            EmailProviderId = provider.Id,
                            From = request.From ?? configuration.FromEmail,
                            To = email,
                            Subject = request.Subject,
                            IsSuccessful = response.Success,
                            Remark = response.Message,
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
                    throw;
                }

                if (recipients.Count == 0) break;
            }

            if (sentEmails.Count > 0)
            {
                await _sentEmailRepository.AddRangeAsync(sentEmails);
            }

            if (recipients.Count > 0)
            {
                _logger.LogWarning("Not enough capacity to send all emails. Remaining: {Count}",
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

            await _emailProviderRepository.UpdateAsync(emailProvider);
        }
    }
}
