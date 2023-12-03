using Mail.Domain.Responses.Errors;

namespace Mail.Domain.Responses;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    
    public Error? Error { get; private set; }
    
    public T? Response { get; private set; }

    public Result(T response)
    {
        Response = response;
        IsSuccess = true;
    }
    
    public Result(Error error)
    {
        Error = error;
        IsSuccess = false;
    }
}