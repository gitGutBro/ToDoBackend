using ToDoBackend.Dtos;
using ToDoBackend.Models;
using ToDoBackend.Utils;

namespace ToDoBackend.Repositories;

public class ToDoRepository(CreateToDoMapper createMapper, UpdateToDoMapper updateMapper) : IToDoRepository
{
    private readonly List<ToDoItem> _toDoItems = [];
    private readonly CreateToDoMapper _createMapper = createMapper;
    private readonly UpdateToDoMapper _updateMapper = updateMapper;

    public async Task<IEnumerable<ToDoItem>> GetAllAsync() =>
        await Task.FromResult(_toDoItems);

    public async Task<ToDoItem?> GetByIdAsync(Guid id) =>
        await Task.FromResult(_toDoItems.FirstOrDefault(item => item.Id == id));

    public Task CreateAsync(CreateToDoItemDto dto)
    {
        ToDoItem item = _createMapper.Map(dto);

        _toDoItems.Add(item);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(UpdateToDoItemDto item)
    {
        ToDoItem? existingItem = _toDoItems.FirstOrDefault(toDoItem => toDoItem.Id == item.Id);

        _updateMapper.Update(existingItem!, item);

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