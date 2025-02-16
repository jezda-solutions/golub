using Golub.Requests;

namespace Golub.Services.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Handles incoming email requests, forwards them to the email distributor for processing,  
        /// and logs relevant details for tracking and debugging.  
        /// 
        /// This method ensures emails are properly distributed and recorded.
        /// </summary>
        /// <param name="request">The email request containing recipient details and message content.</param>
        /// <returns>A task representing the asynchronous operation of sending the email.</returns>
        Task SendEmailAsync(SendEmailRequest request);
    }
}