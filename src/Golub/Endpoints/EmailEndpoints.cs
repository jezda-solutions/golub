using Golub.Requests;
using Golub.Services.Interfaces;

namespace Golub.Endpoints
{
    public static class EmailEndpoints
    {
        public static IEndpointRouteBuilder MapEmailEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/api/emails/send", async (IEmailService emailService, SendEmailRequest request) =>
            {
                await emailService.SendEmailAsync(request);
                return Results.Ok("Email sent successfully.");
            });

            return endpoints;
        }
    }
}