using Golub.Constants;
using Golub.Entities;
using Golub.Interfaces.Repositories;
using Golub.Services.Interfaces;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Golub.Services.SeedServices
{
    public class EmailSeedService(IEmailProviderRepository emailProviderRepository) : IEmailSeedService
    {
        private readonly IEmailProviderRepository _emailProviderRepository 
            = emailProviderRepository ?? throw new ArgumentNullException(nameof(emailProviderRepository));

        public async Task SeedAsync()
        {
            if (await _emailProviderRepository.AnyAsync()) return;

            var fileNames = Directory.GetFiles(SeedConstants.EmailProvidersConfigurationPath);

            var emailProviders = await _emailProviderRepository.GetAsync();

            var cb = new ConcurrentBag<EmailProvider>();

            var tasks = fileNames.Select(async fileName =>
            {
                using StreamReader r = new(fileName);

                var emailProvidersConfigString = await r.ReadToEndAsync();

                var emailProvider = JsonSerializer.Deserialize<EmailProvider>(emailProvidersConfigString);

                if (emailProviders.Any(x => x.Name.Equals(emailProvider.Name))) return;

                cb.Add(emailProvider);
            });

            await Task.WhenAll(tasks);

            await _emailProviderRepository.AddRangeAsync([.. cb]);
        }
    }
}