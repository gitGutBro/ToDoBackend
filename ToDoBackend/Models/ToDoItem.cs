namespace ToDoBackend.Models;

public class ToDoItem
{
    public ToDoItem(string title)
    {
        Id = Guid.NewGuid();
        Title = string.IsNullOrWhiteSpace(title) == false ? 
            title : throw new ArgumentException("Заголовок задачи не может быть пустым.", nameof(title));
        IsCompleted = false;
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public bool IsCompleted { get; set; }

    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Заголовок задачи не может быть пустым.", nameof(newTitle));

        Title = newTitle;
    }

    public void UpdateId(Guid newId)
    {
        if (newId == Guid.Empty)
            throw new ArgumentException("Идентификатор не может быть пустым.", nameof(newId));

        Id = newId;
    }
}