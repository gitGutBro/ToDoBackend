using ToDoBackend.Models.ToDoItem;
using ToDoBackend.ResultPattern;

namespace ToDoBackend.Repositories;

public class ToDoRepository : IToDoRepository
{
    private readonly List<ToDoItem> _toDoItems = [];

    public Task<Result<IEnumerable<ToDoItem>>> GetAllAsync(CancellationToken cancelToken) =>
        Task.FromResult(Result<IEnumerable<ToDoItem>>.Success(_toDoItems));

    public Task<Result<ToDoItem?>> GetByIdAsync(Guid id, CancellationToken cancelToken)
    {
        ToDoItem? result = _toDoItems.FirstOrDefault(item => item.Id == id);

        return Task.FromResult(Result<ToDoItem?>.Success(result));
    }

    public Task<Result<ToDoItem>> CreateAsync(ToDoItem item, CancellationToken cancelToken)
    {
        _toDoItems.Add(item);

        return Task.FromResult(Result<ToDoItem>.Success(item));
    }

    public Task<Result<ToDoItem>> UpdateAsync(ToDoItem item, CancellationToken cancelToken) =>
        Task.FromResult(Result<ToDoItem>.Success(item));

    public Task<Result<ToDoItem>> DeleteAsync(Guid id, CancellationToken cancelToken)
    {
        ToDoItem? item = _toDoItems.FirstOrDefault(item => item.Id == id);

        if (item is null)
            return Task.FromResult(Result<ToDoItem>.Failure(Error.NotFound));

        _toDoItems.Remove(item);
        return Task.FromResult(Result<ToDoItem>.Success(item));
    }
}