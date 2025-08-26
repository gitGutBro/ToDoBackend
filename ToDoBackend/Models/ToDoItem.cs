namespace ToDoBackend.Models;

public class ToDoItem : IModel
{
    public ToDoItem(string title)
    {
        Id = Guid.NewGuid();
        Title = title;
        IsCompleted = false;
    }

    public Guid Id { get; }
    public string Title { get; private set; }
    public bool IsCompleted { get; set; }

    public void UpdateTitle(string newTitle) => 
        Title = newTitle.Trim();
}