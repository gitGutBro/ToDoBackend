using ToDoBackend.Models.ToDoItem;
using ToDoBackend.ResultPattern;

namespace ToDoBackend.Repositories;

public interface IToDoRepository
{
    Task<Result<IEnumerable<ToDoItem>>> GetAllAsync(CancellationToken cancelToken);
    Task<Result<ToDoItem?>> GetByIdAsync(Guid id, CancellationToken cancelToken);
    Task<Result<ToDoItem>> CreateAsync(ToDoItem item, CancellationToken cancelToken);
    Task<Result<ToDoItem>> UpdateAsync(ToDoItem item, CancellationToken cancelToken);
    Task<Result<ToDoItem>> DeleteAsync(Guid id, CancellationToken cancelToken);
}