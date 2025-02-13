using Golub.Requests;

namespace Golub.Responses
{
    public interface IEmailResponse
    {
        bool Success { get; }
        string Message { get; }
    }
}
