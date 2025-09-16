namespace Shared.ResultPattern;

public record class Error(ErrorCode Code, string Description)
{
    public static Error None => new(ErrorCode.None, string.Empty);
    public static Error MissingId => new(ErrorCode.MissingId, "Идентификатор не может быть пустым.");
    public static Error NotFound => new(ErrorCode.NotFound, "Сущность не найдена.");
    public static Error ValidationError => new(ErrorCode.ValidationError, "Ошибка валидации.");
    public static Error OperatinCanceled => new(ErrorCode.OperationCanceled, "Операция была отменена.");
    public static Error DatabaseError => new(ErrorCode.DatabaseError, "Ошибка датабазы.");
    public static Error DatabaseConcurrencyError => new(ErrorCode.DatabaseConcurrencyError, "Ошибка датабазы.");
    public static Error UnknownError => new(ErrorCode.UnknownError, "Неизвестная ошибка.");

    public static Error NotFoundWithId(Guid id) =>
        new(ErrorCode.NotFound, $"Сущность с идентификатором '{id}' не найдена.");

    public static Error DatabaseConcurrencyErrorWithId(Guid id) =>
        new(ErrorCode.DatabaseConcurrencyError, $"Ошибка конкурентного доступа базы данных. Айди сущности: {id}.");
}