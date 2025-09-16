namespace Shared.ResultPattern;

public class Result<TValue>
{
    private readonly TValue _value;
    private readonly Error _error;

    private readonly bool _isSuccess;

    private Result(TValue value)
    {
        _isSuccess = true;
        _value = value;
        _error = Error.None;
    }

    private Result(Error error)
    {
        _isSuccess = false;
        _value = default!;
        _error = error;
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public static Result<TValue> Failure(Error error) => new(error);

    public bool IsSuccess => _isSuccess;
    public bool IsFailure => _isSuccess == false;

    public TValue Value
    {
        get
        {
            if (IsSuccess == false)
                throw new InvalidOperationException($"Result contains error: {_error}");

            return _value;
        }
    }

    public Error Error
    {
        get
        {
            if (IsFailure == false)
                throw new InvalidOperationException("Result does not contain an error.");

            return _error;
        }
    }

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<Error, TResult> failure) =>
        _isSuccess ? success(_value) : failure(_error);
}