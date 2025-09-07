using ToDoBackend.Dtos;
using ToDoBackend.ResultPattern;

namespace ToDoBackend.Services;

public interface IToDoService
{
    Task<Result<IEnumerable<ToDoItemDto>>> GetAllAsync(CancellationToken cancelToken);
    Task<Result<ToDoItemDto?>> GetByIdAsync(Guid id, CancellationToken cancelToken);
    Task<Result<ToDoItemDto>> CreateAsync(CreateToDoItemDto item, CancellationToken cancelToken);
    Task<Result<ToDoItemDto>> UpdateAsync(Guid id, UpdateToDoItemDto item, CancellationToken cancelToken);
    Task<Result<ToDoItemDto>> DeleteAsync(Guid id, CancellationToken cancelToken);
}