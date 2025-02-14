using Golub.Entities;
using Golub.Entities.ProviderConfiguration;
using Golub.Requests;
using Golub.Responses;

namespace Golub.Services.Interfaces
{
    public interface IEmailProvider
    {
        /// <summary>
        /// Email provider name
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Method for sending emails
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IEmailResponse> SendEmailAsync(SendEmailRequest request, EmailProvider provider, BaseEmailProviderConfiguration configuration);
    }
}
