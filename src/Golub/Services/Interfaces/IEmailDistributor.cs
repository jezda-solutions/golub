using Golub.Requests;

namespace Golub.Services.Interfaces
{
    public interface IEmailDistributor
    {
        /// <summary>
        /// Responsible for distribution of emails to all email providers
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task DistributeEmailsByCapacityAsync(SendEmailRequest request);
    }
}
