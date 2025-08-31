namespace ToDoBackend.Models.ToDoItem;

public sealed record class Title
{
    const int MinTitleLength = 1;
    const int MaxTitleLength = 100;

    public Title(string title) => 
        SetValue(title);

    public string Value { get; private set; } = null!;

    public static implicit operator string(Title title) => title.Value;
    public static implicit operator Title(string value) => new(value);

    public void SetValue(string newValue)
    {
        newValue = newValue.Trim();

        ValidateArgument(newValue);

        Value = newValue;
    }

    private static void ValidateArgument(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Заголовок задачи не может быть пустым!", nameof(value));

        if (value.Length < MinTitleLength)
            throw new ArgumentOutOfRangeException($"Минимальное количество символов для задачи: {MinTitleLength}");

        if (value.Length > MaxTitleLength)
            throw new ArgumentOutOfRangeException($"Максимальное количество символов для задачи: {MaxTitleLength}");
    }
}