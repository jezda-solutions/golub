using Golub.Requests;

namespace Golub.Services.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Responsible to receive request anywhere from the application,
        /// to forward it further to the email distributor
        /// and log information
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task SendEmailAsync(SendEmailRequest request);
    }
}