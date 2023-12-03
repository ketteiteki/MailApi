namespace Mail.Domain.Responses.Errors;

public class Error(string message)
{
    public string Message { get; set; } = message;
}