using System.Threading;
using ToDoBackend.Models;

namespace ToDoBackend.Repositories;

public class ToDoRepository : IToDoRepository
{
    private readonly List<ToDoItem> _toDoItems = [];

    public async Task<IEnumerable<ToDoItem>> GetAllAsync(CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            return await Task.FromCanceled<IEnumerable<ToDoItem>>(cancelToken);

        return await Task.FromResult(_toDoItems);
    }

    public async Task<ToDoItem?> GetByIdAsync(Guid id, CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            return await Task.FromCanceled<ToDoItem?>(cancelToken);

        return await Task.FromResult(_toDoItems.FirstOrDefault(item => item.Id == id));
    }

    public Task CreateAsync(ToDoItem item, CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            return Task.FromCanceled(cancelToken);

        _toDoItems.Add(item);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ToDoItem item, CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            return Task.FromCanceled(cancelToken);


        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            cancelToken.ThrowIfCancellationRequested();

        ToDoItem? itemToRemove = await GetByIdAsync(id, cancelToken);

        if (itemToRemove != null)
        {
            if (cancelToken.IsCancellationRequested)
                cancelToken.ThrowIfCancellationRequested();

            _toDoItems.Remove(itemToRemove);
        }
    }
}