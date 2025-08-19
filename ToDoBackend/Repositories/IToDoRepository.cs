using ToDoBackend.Dtos;
using ToDoBackend.Models;

namespace ToDoBackend.Repositories;

public interface IToDoRepository
{
    Task<IEnumerable<ToDoItem>> GetAllAsync();
    Task<ToDoItem?> GetByIdAsync(Guid id);
    Task CreateAsync(CreateToDoItemDto item);
    Task UpdateAsync(UpdateToDoItemDto item);
    Task DeleteAsync(Guid id);
}