namespace Logic.Utils;

public class Result<T>
{
    public T? Value { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }

    private Result(bool isSuccess, T? value, string? errorMessage)
    {
        Value = value;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);
    public static Result<T> Success(T value) => new(true, value, null);
}
