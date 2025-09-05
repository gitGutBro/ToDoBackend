namespace ToDoBackend.ResultPattern;

public enum ErrorCode
{
    None,
    MissingId,
    NotFound,
    ValidationError,
    DatabaseError,
    UnknownError
}