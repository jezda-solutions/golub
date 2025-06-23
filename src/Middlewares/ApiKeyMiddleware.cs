using Golub.Services.ApiKeyServices;

namespace Golub.Middlewares
{
    /// <summary>
    /// Middleware responsible to check the validity of API Key.
    /// </summary>
    /// <param name="next"></param>
    /// <param name="serviceScopeFactory"></param>
    public class ApiKeyMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        private readonly RequestDelegate _next = next;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        public async Task InvokeAsync(HttpContext context)
        {
            // Checks if API Key header is present
            if (!context.Request.Headers.TryGetValue("X-API-Key", out var value))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("API Key is missing.");
                return;
            }

            // Checks if client name header is present
            if (!context.Request.Headers.TryGetValue("X-Client-Name", out var nameValue))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Client name is missing.");
                return;
            }

            using var scope = _serviceScopeFactory.CreateScope();

            var apiKey = value.FirstOrDefault();
            var applicationName = nameValue.FirstOrDefault();
            var apiKeyValidationService = scope.ServiceProvider.GetRequiredService<ApiKeyValidationService>();

            // Checks if API Key is valid and exist in database
            if (!await apiKeyValidationService.IsValidApiKeyAsync(apiKey, applicationName))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Invalid or expired API Key.");
                return;
            }

            await _next(context);
        }
    }
}