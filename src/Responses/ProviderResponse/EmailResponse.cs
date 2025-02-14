namespace Golub.Responses.ProviderResponse
{
    /// <summary>
    /// Generic class for email responses
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Success"></param>
    /// <param name="Message"></param>
    /// <param name="Data"></param>
    public record EmailResponse<T>(bool Success, string Message, T Data = default) : IEmailResponse;
}
