namespace Golub.Responses.ProviderResponse
{
    public record EmailResponse<T>(bool Success, string Message, T Data = default) : IEmailResponse;
}
