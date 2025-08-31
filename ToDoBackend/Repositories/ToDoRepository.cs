using ToDoBackend.Models.ToDoItem;

namespace ToDoBackend.Repositories;

public class ToDoRepository : IToDoRepository
{
    private readonly List<ToDoItem> _toDoItems = [];

    public async Task<IEnumerable<ToDoItem>> GetAllAsync() =>
        await Task.FromResult(_toDoItems);

    public async Task<ToDoItem?> GetByIdAsync(Guid id) =>
        await Task.FromResult(_toDoItems.FirstOrDefault(item => item.Id == id));

    public Task CreateAsync(ToDoItem item)
    {
        _toDoItems.Add(item);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ToDoItem item) =>
        Task.CompletedTask;

    public async Task DeleteAsync(Guid id)
    {
        ToDoItem? itemToRemove = await GetByIdAsync(id);

        if (itemToRemove is not null)
            _toDoItems.Remove(itemToRemove);
    }
}