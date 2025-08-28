namespace ToDoBackend.ResultPattern;

public record class Error(string Code, string Description)
{
    public static Error None => new(string.Empty, string.Empty);
}
