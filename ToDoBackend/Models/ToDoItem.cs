namespace ToDoBackend.Models;

public class ToDoItem(string title) : IModel
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; private set; } = title.Trim();
    public bool IsCompleted { get; set; } = false;

    public void UpdateTitle(string newTitle) => 
        Title = newTitle.Trim();
}