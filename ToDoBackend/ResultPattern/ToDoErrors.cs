namespace ToDoBackend.ResultPattern;

public class ToDoErrors
{
    public static readonly Error MissingId = new("MISSING_ID", "Идентификатор не может быть пустым.");
    public static readonly Error NotFound = new("NOT_FOUND", "Задача не найдена.");
}