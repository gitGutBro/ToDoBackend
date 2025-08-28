using ToDoBackend.Dtos;
using ToDoBackend.Mappers;
using ToDoBackend.Models;
using ToDoBackend.Repositories;

namespace ToDoBackend.Services;

public class ToDoService : IToDoService
{
    private readonly IToDoRepository _toDoRepository;
    private readonly CreateToDoMapper _createMapper;
    private readonly UpdateToDoMapper _updateMapper;

    public ToDoService(IToDoRepository toDoRepository, CreateToDoMapper createMapper, UpdateToDoMapper updateMapper)
    {
        _toDoRepository = toDoRepository ?? throw new ArgumentNullException(nameof(toDoRepository));
        _createMapper = createMapper ?? throw new ArgumentNullException(nameof(createMapper));
        _updateMapper = updateMapper ?? throw new ArgumentNullException(nameof(updateMapper));
    }

    public async Task<IEnumerable<ToDoItem>> GetAllAsync() =>
        await _toDoRepository.GetAllAsync();

    public Task<ToDoItem?> GetByIdAsync(Guid id) =>
        _toDoRepository.GetByIdAsync(id);

    public async Task<ToDoItem> CreateAsync(CreateToDoItemDto dto)
    {
        ToDoItem item = _createMapper.MapToModel(dto);

        await _toDoRepository.CreateAsync(item);
        return item;
    }

    public async Task UpdateAsync(Guid id, UpdateToDoItemDto dto)
    {
        ToDoItem? item = await _toDoRepository.GetByIdAsync(id);

        _updateMapper.UpdateModel(item, dto);
        await _toDoRepository.UpdateAsync(item);
    }

    public Task DeleteAsync(Guid id) =>
        _toDoRepository.DeleteAsync(id);
}