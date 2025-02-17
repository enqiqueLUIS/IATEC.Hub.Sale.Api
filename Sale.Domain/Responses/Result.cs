using System.Net;

namespace Sale.Domain.Responses;

public class Result<T>
{
    public T Data { get; private set; }
    public bool IsSuccess { get; private set; }
    public HttpStatusCode StatusCode { get; private set; }
    public string Message { get; private set; }
    public List<string>Errors { get; private set; }
    
    private Result(T data, bool isSuccess, HttpStatusCode code, string message, List<string> errors)
    {
        Data = data;
        IsSuccess = isSuccess;
        StatusCode = code;
        Message = message;
        Errors = errors;
    }
    
    public Result()
    {
        IsSuccess = true;
        StatusCode = HttpStatusCode.OK;
        Errors = new List<string>();
    }
    
    public Result(string errorMessage)
    {
        IsSuccess = false;
        Message = errorMessage;
        StatusCode = HttpStatusCode.BadRequest;
        Errors = new List<string> { errorMessage };
    }
    
    public Result(HttpStatusCode statusCode, string message = null)
    {
        IsSuccess = statusCode == HttpStatusCode.OK;
        StatusCode = statusCode;
        Message = message;
        Errors = IsSuccess ? new List<string>() : new List<string> { message };
    }
    
    public static Result<T> Success(
        T data,
        HttpStatusCode code
    ) => new Result<T>(
        data,
        true,
        code,
        HttpStatusMessages.GetMessage((int)code),
        new List<string>()
    );
    public static Result<T> Failure(
        List<string> errors,
        HttpStatusCode code
    ) => new Result<T>(
        default(T),
        false,
        code,
        HttpStatusMessages.GetMessage((int)code),
        errors
    );
    
}