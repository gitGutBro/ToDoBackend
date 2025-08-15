using ToDoBackend.Models;

namespace ToDoBackend.Repositories;

public interface IToDoRepository
{
    Task<IEnumerable<ToDoItem>> GetAllAsync();
    Task<ToDoItem?> GetByIdAsync(Guid id);
    Task AddAsync(ToDoItem item);
    Task UpdateAsync(ToDoItem item);
    Task DeleteAsync(Guid id);
}