namespace Shared.ResultPattern;

public enum ErrorCode
{
    None,
    MissingId,
    NotFound,
    ValidationError,
    OperationCanceled,
    DatabaseError,
    DatabaseConcurrencyError,
    UnknownError,
}