using Golub.Entities;
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
        Task<IEmailResponse> SendEmailAsync(SendEmailRequest request, email_provider provider);
    }
}
