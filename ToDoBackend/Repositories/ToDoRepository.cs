using ToDoBackend.Dtos;
using ToDoBackend.Models;

namespace ToDoBackend.Repositories;

public class ToDoRepository : IToDoRepository
{
    private readonly List<ToDoItem> _toDoItems = [];

    public async Task<IEnumerable<ToDoItem>> GetAllAsync() =>
        await Task.FromResult(_toDoItems);

    public async Task<ToDoItem?> GetByIdAsync(Guid id) =>
        await Task.FromResult(_toDoItems.FirstOrDefault(item => item.Id == id));

    public async Task AddAsync(ToDoItem item) =>
        await Task.Run(() => _toDoItems.Add(item));

    public Task UpdateAsync(UpdateToDoItemDto item)
    {
        ToDoItem? existingItem = _toDoItems.FirstOrDefault(toDoItem => toDoItem.Id == item.Id);

        if (existingItem != null)
        {
            existingItem.UpdateTitle(item.Title);
            existingItem.IsCompleted = item.IsCompleted;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        ToDoItem? itemToRemove = _toDoItems.FirstOrDefault(toDoItem => toDoItem.Id == id);

        if (itemToRemove != null)
            _toDoItems.Remove(itemToRemove);

        return Task.CompletedTask;
    }
}