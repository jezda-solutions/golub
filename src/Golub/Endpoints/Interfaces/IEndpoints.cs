namespace Golub.Endpoints.Interfaces
{
    /// <summary>
    /// Interface for registering endpoints
    /// </summary>
    public interface IEndpoints
    {
        void RegisterEndpoints(IEndpointRouteBuilder app);
    }
}
