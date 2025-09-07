using FluentValidation.Results;
using ToDoBackend.Dtos;
using ToDoBackend.Mappers;
using ToDoBackend.Models.ToDoItem;
using ToDoBackend.Validators;
using ToDoBackend.Repositories;
using ToDoBackend.ResultPattern;
using ToDoBackend.Extensions;

namespace ToDoBackend.Services;

internal class ToDoService(IToDoRepository toDoRepository,
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
        ToDoItem item = _createMapper.MapToModel(dto);

        ValidationResult validationResults = _validator.Validate(item);

        if (validationResults.TryCheckValidation() == false)
            return await Task.FromResult(Result<ToDoItemDto>.Failure(Error.ValidationError));

        Result<ToDoItem> created = await _toDoRepository.CreateAsync(item, cancellationToken);

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

        _updateMapper.UpdateModel(branch.Item, dto);

        ValidationResult validation = _validator.Validate(branch.Item);

        if (validation.TryCheckValidation() == false)
            return Result<ToDoItemDto>.Failure(Error.ValidationError);

        Result<ToDoItem> updateResult = await _toDoRepository.UpdateAsync(branch.Item, cancellationToken);

        return updateResult.Match
        (
            updated => Result<ToDoItemDto>.Success(_toDoMapper.MapToDto(updated)),
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
