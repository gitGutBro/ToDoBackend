using ToDoBackend.Mappers;

namespace ToDoBackend.Models;

public class ToDoItem : IMappable
{
    public ToDoItem(string title)
    {
        Id = Guid.NewGuid();
        Title = string.IsNullOrWhiteSpace(title) == false ? 
            title : throw new ArgumentException("Заголовок задачи не может быть пустым.", nameof(title));
        IsCompleted = false;
    }

    public Guid Id { get; }
    public string Title { get; private set; }
    public bool IsCompleted { get; set; }

    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Заголовок задачи не может быть пустым.", nameof(newTitle));

        Title = newTitle;
    }
}