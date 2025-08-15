using ToDoBackend.Dtos;
using ToDoBackend.Models;
using ToDoBackend.Repositories;

namespace ToDoBackend.Services;

public class ToDoService : IToDoService
{
    private readonly IToDoRepository _toDoRepository;

    public ToDoService(IToDoRepository toDoRepository) => 
        _toDoRepository = toDoRepository ?? throw new ArgumentNullException(nameof(toDoRepository));

    public async Task<IEnumerable<ToDoItem>> GetAllAsync()
    {
        return await _toDoRepository.GetAllAsync();
    }

    public Task<ToDoItem?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Идентификатор не может быть пустым.", nameof(id));

        return _toDoRepository.GetByIdAsync(id);
    }

    public Task AddAsync(ToDoItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "Элемент не может быть null.");

        return _toDoRepository.AddAsync(item);
    }

    public Task UpdateAsync(UpdateToDoItemDto item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "Элемент не может быть null.");
        
        return _toDoRepository.UpdateAsync(item);
    }

    public Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Идентификатор не может быть пустым.", nameof(id));

        return _toDoRepository.DeleteAsync(id);
    }
}