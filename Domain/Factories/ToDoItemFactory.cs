using Domain.Entities.ToDoItem;

namespace Domain.Factories;

public class ToDoItemFactory : IToDoItemFactory
{
    public ToDoItem Create(string title, string? description = null) =>
        new(title, description);
}