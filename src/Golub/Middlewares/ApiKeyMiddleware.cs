using Golub.Services.ApiKeyServices;
namespace Golub.Middlewares
{
    public class ApiKeyMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        private readonly RequestDelegate _next 
            = next ?? throw new ArgumentNullException(nameof(next));
        private readonly IServiceScopeFactory _serviceScopeFactory 
            = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("X-API-Key", out var value))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("API Key is missing.");
                return;
            }

            var apiKey = value.FirstOrDefault();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var apiKeyValidationService = scope.ServiceProvider.GetRequiredService<ApiKeyValidationService>();

                if (string.IsNullOrEmpty(apiKey) || !await apiKeyValidationService.IsValidApiKeyAsync(apiKey))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Invalid or expired API Key.");
                    return;
                }
            }

            await _next(context);
        }
    }
}