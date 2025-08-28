using ToDoBackend.Dtos;
using ToDoBackend.Models;

namespace ToDoBackend.Services;

public interface IToDoService
{
    Task<IEnumerable<ToDoItem>> GetAllAsync(CancellationToken cancelToken);
    Task<ToDoItem?> GetByIdAsync(Guid id, CancellationToken cancelToken);
    Task<ToDoItem> CreateAsync(CreateToDoItemDto item, CancellationToken cancelToken);
    Task UpdateAsync(Guid id, UpdateToDoItemDto item, CancellationToken cancelToken);
    Task DeleteAsync(Guid id, CancellationToken cancelToken);
}