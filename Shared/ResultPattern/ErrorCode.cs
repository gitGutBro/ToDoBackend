namespace Shared.ResultPattern;

public enum ErrorCode
{
    None,
    MissingId,
    NullReference,
    NotFound,
    ValidationError,
    PublishError,
    OperationCanceled,
    DatabaseError,
    DatabaseConcurrencyError,
    UnknownError,
}