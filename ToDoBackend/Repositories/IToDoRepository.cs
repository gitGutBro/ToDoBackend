using ToDoBackend.Models;

namespace ToDoBackend.Repositories;

public interface IToDoRepository
{
    Task<IEnumerable<ToDoItem>> GetAllAsync(CancellationToken cancelToken);
    Task<ToDoItem?> GetByIdAsync(Guid id, CancellationToken cancelToken);
    Task CreateAsync(ToDoItem item, CancellationToken cancelToken);
    Task UpdateAsync(ToDoItem item, CancellationToken cancelToken);
    Task DeleteAsync(Guid id, CancellationToken cancelToken);
}