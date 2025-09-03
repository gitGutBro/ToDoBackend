using ToDoBackend.Dtos;
using ToDoBackend.Models.ToDoItem;
using ToDoBackend.ResultPattern;

namespace ToDoBackend.Services;

public interface IToDoService
{
    Task<Result<IEnumerable<ToDoItem>>> GetAllAsync(CancellationToken cancelToken);
    Task<Result<ToDoItem?>> GetByIdAsync(Guid id, CancellationToken cancelToken);
    Task<Result<ToDoItem>> CreateAsync(CreateToDoItemDto item, CancellationToken cancelToken);
    Task<Result<ToDoItem>> UpdateAsync(Guid id, UpdateToDoItemDto item, CancellationToken cancelToken);
    Task<Result<ToDoItem>> DeleteAsync(Guid id, CancellationToken cancelToken);
}