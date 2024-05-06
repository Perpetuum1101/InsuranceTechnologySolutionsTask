namespace Application.Result;

public class Result<T> where T : class
{
    private Result(T? response, bool isSuccess, string? errors = null)
    {
        Response = response;
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public T? Response { get; init; }
    public string? Errors { get; init; }
    public bool IsSuccess { get; init; }

    public static Result<T> Success(T result)
    {
        return new Result<T>(result, true, null);
    }

    public static Result<T> Failure(IEnumerable<string>? errors = null)
    {
        string? errorsString = null;
        if (errors != null)
        {
            errorsString = string.Join(", ", errors);
        }

        return new Result<T>(null, false, errorsString);
    }
}
