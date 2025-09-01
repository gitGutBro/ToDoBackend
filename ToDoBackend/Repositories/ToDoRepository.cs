using System.Threading;
using ToDoBackend.Models;
using ToDoBackend.ResultPattern;

namespace ToDoBackend.Repositories;

public class ToDoRepository : IToDoRepository
{
    private readonly List<ToDoItem> _toDoItems = [];

    public Task<Result<IEnumerable<ToDoItem>>> GetAllAsync(CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            return Task.FromCanceled<Result<IEnumerable<ToDoItem>>>(cancelToken);

        return Task.FromResult(Result<IEnumerable<ToDoItem>>.Success(_toDoItems));
    }

    public Task<Result<ToDoItem?>> GetByIdAsync(Guid id, CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            return Task.FromCanceled<Result<ToDoItem?>>(cancelToken);

        ToDoItem? result = _toDoItems.FirstOrDefault(item => item.Id == id);

        return Task.FromResult(Result<ToDoItem?>.Success(result));
    }

    public Task<Result<ToDoItem>> CreateAsync(ToDoItem item, CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            return Task.FromCanceled<Result<ToDoItem>>(cancelToken);

        _toDoItems.Add(item);

        return Task.FromResult(Result<ToDoItem>.Success(item));
    }

    public Task<Result<ToDoItem>> UpdateAsync(ToDoItem item, CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            return Task.FromCanceled<Result<ToDoItem>>(cancelToken);

        return Task.FromResult(Result<ToDoItem>.Success(item));
    }

    public Task<Result<ToDoItem>> DeleteAsync(Guid id, CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            return Task.FromCanceled<Result<ToDoItem>>(cancelToken);

        ToDoItem? item = _toDoItems.FirstOrDefault(item => item.Id == id);

        if (item is null)
            return Task.FromResult(Result<ToDoItem>.Failure(Error.NotFound));

        _toDoItems.Remove(item);
        return Task.FromResult(Result<ToDoItem>.Success(item));
    }
}