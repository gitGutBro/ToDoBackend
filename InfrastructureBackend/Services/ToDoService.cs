using FluentValidation.Results;
using Shared.ResultPattern;
using Shared.Extensions;
using Domain.Entities.ToDoItem;
using ApplicationBackend.Repositories;
using ApplicationBackend.Validators;
using ApplicationBackend.Mappers;
using ApplicationBackend.Dtos;
using ApplicationBackend.Services;

namespace InfrastructureBackend.Services;

public class ToDoService(IToDoRepository toDoRepository,
                         ToDoItemValidator validator,
                         IMapper<ToDoItem, CreateToDoItemDto> createMapper,
                         IUpdateMapper<ToDoItem, UpdateToDoItemDto> updateMapper,
                         IMapper<ToDoItem, ToDoItemDto> toDoMapper) : IToDoService
{
    private readonly IToDoRepository _toDoRepository = toDoRepository ?? throw new ArgumentNullException(nameof(toDoRepository));
    private readonly ToDoItemValidator _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    private readonly IMapper<ToDoItem, CreateToDoItemDto> _createMapper = createMapper ?? throw new ArgumentNullException(nameof(createMapper));
    private readonly IMapper<ToDoItem, ToDoItemDto> _toDoMapper = toDoMapper ?? throw new ArgumentNullException(nameof(toDoMapper));
    private readonly IUpdateMapper<ToDoItem, UpdateToDoItemDto> _updateMapper = updateMapper ?? throw new ArgumentNullException(nameof(updateMapper));

    public async Task<Result<IEnumerable<ToDoItemDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        Result<IEnumerable<ToDoItem>> itemsResult = await _toDoRepository.GetAllAsync(cancellationToken);

        return itemsResult.Match
        (
            items => Result<IEnumerable<ToDoItemDto>>.Success([.. items.Select(_toDoMapper.MapToDto)]),
            Result<IEnumerable<ToDoItemDto>>.Failure
        );
    }

    public async Task<Result<ToDoItemDto?>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Result<ToDoItem?> repoResult = await _toDoRepository.GetByIdAsync(id, cancellationToken);

        return repoResult.Match
        (
            item => item is null
                ? Result<ToDoItemDto?>.Failure(Error.NotFoundWithId(id))
                : Result<ToDoItemDto?>.Success(_toDoMapper.MapToDto(item)),
            Result<ToDoItemDto?>.Failure
        );
    }

    public async Task<Result<ToDoItemDto>> CreateAsync(CreateToDoItemDto dto, CancellationToken cancellationToken)
    {
        Result<ToDoItem> item = _createMapper.MapToModel(dto);

        ValidationResult validationResults = _validator.Validate(item.Value);

        if (validationResults.TryCheckValidation() == false)
            return await Task.FromResult(Result<ToDoItemDto>.Failure(Error.ValidationError));

        Result<ToDoItem> created = await _toDoRepository.CreateAsync(item.Value, cancellationToken);

        return created.Match
        (
            createdItem => Result<ToDoItemDto>.Success(_toDoMapper.MapToDto(createdItem)),
            Result<ToDoItemDto>.Failure
        );
    }

    public async Task<Result<ToDoItemDto>> UpdateAsync(Guid id, UpdateToDoItemDto dto, CancellationToken cancellationToken)
    {
        Result<ToDoItem?> getResult = await _toDoRepository.GetByIdAsync(id, cancellationToken);

        (bool Succeeded, ToDoItem? Item, Error Error) branch = getResult.Match
        (
            existing => (Succeeded: true, Item: existing, Error: Error.None),
            error => (Succeeded: false, Item: null, Error: error)
        );

        if (branch.Succeeded == false)
            return Result<ToDoItemDto>.Failure(branch.Error);

        if (branch.Item is null)
            return Result<ToDoItemDto>.Failure(Error.NotFoundWithId(id));

        Result<bool> updateResult = _updateMapper.UpdateEntity(branch.Item, dto);

        if (updateResult.IsFailure)
            return Result<ToDoItemDto>.Failure(updateResult.Error);

        ValidationResult validation = _validator.Validate(branch.Item);

        if (validation.TryCheckValidation() == false)
            return Result<ToDoItemDto>.Failure(Error.ValidationError);

        Result<ToDoItem> updatedItemResult = await _toDoRepository.UpdateAsync(branch.Item, cancellationToken);

        return updateResult.Match
        (
            _ => Result<ToDoItemDto>.Success(_toDoMapper.MapToDto(updatedItemResult.Value)),
            Result<ToDoItemDto>.Failure
        );
    }

    public async Task<Result<ToDoItemDto>> DeleteAsync(Guid id, CancellationToken cancelToken)
    {
        Result<ToDoItem> deleted = await _toDoRepository.DeleteAsync(id, cancelToken);

        return deleted.Match
        (
            item => Result<ToDoItemDto>.Success(_toDoMapper.MapToDto(item)),
            Result<ToDoItemDto>.Failure
        );
    }
}