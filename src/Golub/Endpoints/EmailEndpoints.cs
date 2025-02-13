using Golub.Requests;
using Golub.Services.Interfaces;

namespace Golub.Endpoints
{
    /// <summary>
    /// Email endpoints
    /// Class contains all email endpoints
    /// </summary>
    public static class EmailEndpoints
    {
        public static IEndpointRouteBuilder MapEmailEndpoints(this IEndpointRouteBuilder endpoints)
        {
            // Sending email endpoint
            endpoints.MapPost("/api/emails/send", async (IEmailService emailService, SendEmailRequest request) =>
            {
                await emailService.SendEmailAsync(request);
                return Results.Ok("Email sent successfully.");
            });

            return endpoints;
        }
    }
}