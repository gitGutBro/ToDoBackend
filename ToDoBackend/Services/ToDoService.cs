using FluentValidation.Results;
using ToDoBackend.Dtos;
using ToDoBackend.Mappers;
using ToDoBackend.Models;
using ToDoBackend.Repositories;
using ToDoBackend.Validators;

namespace ToDoBackend.Services;

public class ToDoService(IToDoRepository toDoRepository,
                         ToDoItemValidator validator,
                         CreateToDoMapper createMapper, 
                         UpdateToDoMapper updateMapper) : IToDoService
{
    private readonly IToDoRepository _toDoRepository = toDoRepository ?? throw new ArgumentNullException(nameof(toDoRepository));
    private readonly ToDoItemValidator _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    private readonly CreateToDoMapper _createMapper = createMapper ?? throw new ArgumentNullException(nameof(createMapper));
    private readonly UpdateToDoMapper _updateMapper = updateMapper ?? throw new ArgumentNullException(nameof(updateMapper));

    public async Task<IEnumerable<ToDoItem>> GetAllAsync() =>
        await _toDoRepository.GetAllAsync();


    public Task<ToDoItem?> GetByIdAsync(Guid id) =>
        _toDoRepository.GetByIdAsync(id);

    public async Task<ToDoItem> CreateAsync(CreateToDoItemDto dto)
    {
        ToDoItem item = _createMapper.MapToModel(dto);

        ValidationResult validationResults = _validator.Validate(item);

        CheckValidation(validationResults);

        await _toDoRepository.CreateAsync(item);
        return item;
    }

    public async Task UpdateAsync(Guid id, UpdateToDoItemDto dto)
    {
        ToDoItem? item = await _toDoRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Задача с айди {id} не найдена!");
        _updateMapper.UpdateModel(item, dto);

        ValidationResult validationResults = _validator.Validate(item);

        CheckValidation(validationResults);
        
        await _toDoRepository.UpdateAsync(item);
    }

    public async Task DeleteAsync(Guid id)
    {
        ToDoItem? item = await _toDoRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Задача с айди {id} не найдена!");

        await _toDoRepository.DeleteAsync(id);
    }

    private static void CheckValidation(ValidationResult results)
    {
        if (results.IsValid == false)
        {
            foreach (ValidationFailure? failure in results.Errors)
                Console.WriteLine($"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage}");

            throw new FluentValidation.ValidationException(results.Errors);
        }
    }
}