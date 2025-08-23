namespace ToDoBackend.Models;

public sealed record class Title
{
    public Title(string title) => 
        SetValue(title);

    public string Value { get; private set; } = null!;

    public static implicit operator string(Title title) => title.Value;
    public static implicit operator Title(string value) => new(value);

    public void SetValue(string newValue)
    {
        ValidateArgument(newValue);

        Value = newValue;
    }

    private static void ValidateArgument(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Заголовок задачи не может быть пустым.", nameof(value));
    }
}