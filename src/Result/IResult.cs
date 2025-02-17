namespace Golub.Result
{
    /// <summary>
    /// Interface for results
    /// Represents the result of an operation
    /// </summary>
    public interface IResult
    {
        List<string> Messages { get; set; }

        bool Succeeded { get; set; }
    }


    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}