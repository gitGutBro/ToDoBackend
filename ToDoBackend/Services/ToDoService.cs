using FluentValidation.Results;
using ToDoBackend.Dtos;
using ToDoBackend.Mappers;
using ToDoBackend.Models.ToDoItem;
using ToDoBackend.Validators;
using ToDoBackend.Repositories;
using ToDoBackend.ResultPattern;
using ToDoBackend.Extensions;

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

    public async Task<Result<IEnumerable<ToDoItem>>> GetAllAsync(CancellationToken cancelToken) =>
        await _toDoRepository.GetAllAsync(cancelToken);

    public async Task<Result<ToDoItem?>> GetByIdAsync(Guid id, CancellationToken cancelToken) =>
        await _toDoRepository.GetByIdAsync(id, cancelToken);

    public async Task<Result<ToDoItem>> CreateAsync(CreateToDoItemDto dto, CancellationToken cancelToken)
    {
        ToDoItem item = _createMapper.MapToModel(dto);

        ValidationResult validationResults = _validator.Validate(item);

        if (validationResults.TryCheckValidation() == false)
            return await Task.FromResult(Result<ToDoItem>.Failure(Error.ValidationError));

        return await _toDoRepository.CreateAsync(item, cancelToken);
    }

    public async Task<Result<ToDoItem>> UpdateAsync(Guid id, UpdateToDoItemDto dto, CancellationToken cancelToken)
    {
        Result<ToDoItem?> getResult = await _toDoRepository.GetByIdAsync(id, cancelToken);

        Task<Result<ToDoItem>> matchResult = getResult.Match(
            existingToDo =>
            {
                if (existingToDo is null)
                    return Task.FromResult(Result<ToDoItem>.Failure(Error.NotFound));

                _updateMapper.UpdateModel(existingToDo, dto);

                ValidationResult validationResults = _validator.Validate(existingToDo);
                
                if (validationResults.TryCheckValidation() == false)
                    return Task.FromResult(Result<ToDoItem>.Failure(Error.ValidationError));

                return _toDoRepository.UpdateAsync(existingToDo, cancelToken);
            },
            error =>
            {
                return Task.FromResult(Result<ToDoItem>.Failure(error));
            }
        );

        return await matchResult;
    }

    public async Task<Result<ToDoItem>> DeleteAsync(Guid id, CancellationToken cancelToken) =>
        await _toDoRepository.DeleteAsync(id, cancelToken);
}
