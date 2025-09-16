using Domain.Entities.ToDoItem;

namespace Domain.Factories;

public interface IToDoItemFactory
{
    ToDoItem Create(string title, string? description = null);
}