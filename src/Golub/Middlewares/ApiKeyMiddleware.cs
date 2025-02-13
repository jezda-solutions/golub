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
        private readonly RequestDelegate _next
            = next ?? throw new ArgumentNullException(nameof(next));
        private readonly IServiceScopeFactory _serviceScopeFactory
            = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

        public async Task InvokeAsync(HttpContext context)
        {
            // Checks if API Key header is present
            if (!context.Request.Headers.TryGetValue("X-API-Key", out var value))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("API Key is missing.");
                return;
            }

            var encryptedApiKey = value.FirstOrDefault();

            using var scope = _serviceScopeFactory.CreateScope();
            var decryptionService = scope.ServiceProvider.GetRequiredService<ApiKeyService>();

            // Decryption of API Key
            var apiKey = decryptionService.DecryptApiKey(encryptedApiKey);
            if (string.IsNullOrEmpty(apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid API Key format.");
                return;
            }

            var apiKeyValidationService = scope.ServiceProvider.GetRequiredService<ApiKeyValidationService>();

            // Checks if API Key is valid and exist in database
            if (!await apiKeyValidationService.IsValidApiKeyAsync(apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Invalid or expired API Key.");
                return;
            }

            await _next(context);
        }
    }
}