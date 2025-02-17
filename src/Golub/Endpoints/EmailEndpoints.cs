using Golub.Endpoints.Interfaces;
using Golub.Requests;
using Golub.Services.Interfaces;

namespace Golub.Endpoints
{
    /// <summary>
    /// Email endpoints
    /// Class contains all email endpoints
    /// </summary>
    public class EmailEndpoints : IEndpoints
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            // Sending email endpoint
            app.MapPost("/api/emails/send", async (IEmailService emailService, SendEmailRequest request) =>
            {
                try
                {
                    await emailService.SendEmailAsync(request);
                    return Results.Ok("Email sent successfully.");
                }
                catch
                {
                    return Results.Problem(
                        detail: "An error occurred while sending the email.",
                        statusCode: StatusCodes.Status500InternalServerError
                    );
                }
            });
        }
    }
}