using Golub.Requests;

namespace Golub.Services.Interfaces
{
    public interface IEmailDistributor
    {
        /// <summary>
        /// Handles the distribution of outgoing emails across multiple email providers.
        /// The emails received in the request are assigned to different providers based on their capacity,
        /// ensuring efficient distribution and delivery.
        /// </summary>
        /// <param name="request">The email request containing recipient details and message content.</param>
        /// <returns>A task representing the asynchronous operation of email distribution.</returns>
        Task DistributeEmailsByCapacityAsync(SendEmailRequest request);

    }
}
