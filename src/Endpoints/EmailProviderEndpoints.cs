using Golub.Endpoints.Interfaces;
using Golub.Handlers.EmailProvider;
using Microsoft.AspNetCore.Mvc;

namespace Golub.Endpoints
{
    /// <summary>
    /// Email provider endpoints
    /// Class contains all email provider endpoints
    /// </summary>
    public class EmailProviderEndpoints : IEndpoints
    {
        private static string BasePath => "/api/email-providers";

        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            #region GET

            // Endpoint for getting all email providers
            app.MapGet(BasePath, async ([FromServices] GetAllEmailProvidersHandler handler) =>
            {
                var result = await handler.Handle();

                return result.Succeeded
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            });

            // Endpoint for getting email provider by id
            app.MapGet($"{BasePath}/{{id:guid}}", async ([FromServices] GetEmailProviderByIdHandler handler, Guid id) =>
            {
                var result = await handler.Handle(id);

                return result.Succeeded
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            });

            // Endpoint for getting email provider by name
            app.MapGet($"{BasePath}/{{name}}", async ([FromServices] GetEmailProviderByNameHandler handler, string name) =>
            {
                var result = await handler.Handle(name);

                return result.Succeeded
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            });

            #endregion

            #region POST

            // Endpoint for adding email provider
            app.MapPost(BasePath, async ([FromServices] AddEmailProviderHandler handler, [FromBody] AddEmailProviderCommand command) =>
            {
                var result = await handler.Handle(command);

                return result.Succeeded
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            });

            #endregion

            #region PUT

            // Endpoint for updating email provider
            app.MapPut(BasePath, async ([FromServices] UpdateEmailProviderHandler handler, [FromBody] UpdateEmailProviderCommand command) =>
            {
                var result = await handler.Handle(command);

                return result.Succeeded
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            });

            // Endpoint for updating email provider is active status
            app.MapPut($"{BasePath}/is-active", async ([FromServices] UpdateEmailProviderIsActiveHandler handler,
                                                       [FromBody] UpdateEmailProviderIsActiveCommand command) =>
            {
                var result = await handler.Handle(command);

                return result.Succeeded
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            });

            #endregion

            #region DELETE

            // Endpoint for soft deleting email provider
            app.MapDelete($"{BasePath}/{{id}}", async ([FromServices] SoftDeleteEmailProviderHandler handler, Guid id) =>
            {
                var result = await handler.Handle(id);

                return result.Succeeded
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            });

            #endregion
        }
    }
}