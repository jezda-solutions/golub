namespace Golub.Responses
{
    /// <summary>
    /// Email response interface
    /// </summary>
    public interface IEmailResponse
    {
        bool Success { get; }
        string Message { get; }
    }
}
