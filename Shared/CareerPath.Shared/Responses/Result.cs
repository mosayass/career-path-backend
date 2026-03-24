namespace CareerPath.Shared.Responses;

// 1. The new Enum to classify business errors
public enum ErrorType
{
    None = 0,
    Failure = 1,      // Generic failure (maps to 400 Bad Request)
    NotFound = 2,     // Maps to 404 Not Found
    Validation = 3,   // Maps to 400 Bad Request
    Conflict = 4,     // Maps to 409 Conflict
    Forbidden = 5,    // Maps to 403 Forbidden
    Unauthorized = 6  // Maps to 401 Unauthorized
}
public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }
    public ErrorType ErrorType { get; } // Added property

    protected Result(bool isSuccess, ErrorType errorType, string error)
    {
        IsSuccess = isSuccess;
        ErrorType = errorType;
        Error = error;
    }

    public static Result Success() => new(true, ErrorType.None, string.Empty);
    public static Result Failure(string error) => new(false, ErrorType.Failure, error);
    // NEW EXTENSION: Your new MediatR handlers can now use this specific overload
    public static Result Failure(ErrorType errorType, string error) => new(false, errorType, error);
}

public class Result<T> : Result
{
    public T? Value { get; }

    protected Result(T? value, bool isSuccess, ErrorType errorType, string error)
        : base(isSuccess, errorType, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(value, true, ErrorType.None, string.Empty);
    public static Result<T> Failure(string error) => new(default, false, ErrorType.Failure, error);
    public static Result<T> Failure(ErrorType errorType, string error) => new(default, false, errorType, error);
}