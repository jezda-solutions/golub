using Golub.Requests;

namespace Golub.Responses
{
    public interface IResponse
    {
        bool Success { get; }
        string Message { get; }
    }
}
