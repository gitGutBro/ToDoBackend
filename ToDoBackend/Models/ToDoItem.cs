namespace ToDoBackend.Models;

public class ToDoItem(string title) : IModel
{
    public Guid Id { get; } = Guid.NewGuid();
    public Title Title { get; } = new Title(title.Trim());
    public bool IsCompleted { get; set; } = false;

    public void UpdateTitle(string newTitle) => 
        Title.SetValue(newTitle);
}