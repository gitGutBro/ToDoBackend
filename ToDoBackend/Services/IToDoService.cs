using ToDoBackend.Dtos;
using ToDoBackend.Models.ToDoItem;

namespace ToDoBackend.Services;

public interface IToDoService
{
    Task<IEnumerable<ToDoItem>> GetAllAsync();
    Task<ToDoItem?> GetByIdAsync(Guid id);
    Task<ToDoItem> CreateAsync(CreateToDoItemDto item);
    Task UpdateAsync(Guid id, UpdateToDoItemDto item);
    Task DeleteAsync(Guid id);
}