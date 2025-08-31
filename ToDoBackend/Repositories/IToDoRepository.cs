using ToDoBackend.Dtos;
using ToDoBackend.Models.ToDoItem;

namespace ToDoBackend.Repositories;

public interface IToDoRepository
{
    Task<IEnumerable<ToDoItem>> GetAllAsync();
    Task<ToDoItem?> GetByIdAsync(Guid id);
    Task CreateAsync(ToDoItem item);
    Task UpdateAsync(ToDoItem item);
    Task DeleteAsync(Guid id);
}