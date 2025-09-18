using ApplicationBackend.Dtos;
using ApplicationBackend.Events;
using ApplicationBackend.Mappers;
using ApplicationBackend.Repositories;
using Domain.Entities.ToDoItem;
using Domain.Topics;
using Domain.Validators;
using NodaTime;
using Serilog;
using Shared.Extensions;
using Shared.ResultPattern;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace ApplicationBackend.Services;

public class ToDoService(IToDoRepository toDoRepository,
                         IEventPublisher publisher,
                         ToDoItemValidator validator,
                         IMapper<ToDoItem, CreateToDoItemDto> createMapper,
                         IUpdateMapper<ToDoItem, UpdateToDoItemDto> updateMapper,
                         IMapper<ToDoItem, ToDoItemDto> toDoMapper) : IToDoService
{
    private readonly IToDoRepository _toDoRepository = toDoRepository ?? throw new ArgumentNullException(nameof(toDoRepository));
    private readonly IEventPublisher _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
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

        Result<ToDoItem> createdResult = await _toDoRepository.CreateAsync(item.Value, cancellationToken);

        if (createdResult.IsFailure)
            return Result<ToDoItemDto>.Failure(Error.NullReference);

        ToDoItem createdItem = createdResult.Value;
        ToDoItemDto dtoResult = _toDoMapper.MapToDto(createdItem);

        ToDoItemCreatedTopic createdEvent = new
        (
            TopicId: Guid.NewGuid(),

            createdItem.Id, createdItem.Title,
            createdItem.AuditInfo.CreatedAt,
            createdItem.ScheduleInfo.DueDate, createdItem.ScheduleInfo.DueTime,
            createdItem.ScheduleInfo.TimeZoneId, createdItem.ScheduleInfo.ScheduledAt,

            TopicCreatedAt: SystemClock.Instance.GetCurrentInstant()
        );

        Result<ToDoItemCreatedTopic> publishResult = await _publisher.PublishAsync(createdEvent, cancellationToken);

        if (publishResult.IsFailure)
        {
            Log.Error($"Ошибка во время публикации сообщения в брокер сообщений! Ошибка: {publishResult.Error.Code}");
            return Result<ToDoItemDto>.Failure(Error.PublishError);
        }

        return Result<ToDoItemDto>.Success(dtoResult);
    }

    public async Task<Result<ToDoItemDto>> UpdateAsync(Guid id, UpdateToDoItemDto dto, CancellationToken cancellationToken)
    {
        // 1) Получаем сущность
        Result<ToDoItem?> getResult = await _toDoRepository.GetByIdAsync(id, cancellationToken);

        if (getResult.IsFailure)
            return Result<ToDoItemDto>.Failure(getResult.Error);

        ToDoItem? itemToUpdate = getResult.Value;

        if (itemToUpdate is null)
            return Result<ToDoItemDto>.Failure(Error.NotFoundWithId(id));

        // 2) Маппим апдейты
        Result<bool> updateResult = _updateMapper.UpdateEntity(itemToUpdate, dto);

        if (updateResult.IsFailure)
            return Result<ToDoItemDto>.Failure(updateResult.Error);

        // 3) Если изменений нет — можно не писать в БД
        if (updateResult.Value == false)
            return Result<ToDoItemDto>.Success(_toDoMapper.MapToDto(itemToUpdate));

        // 4) Валидация
        ValidationResult validation = _validator.Validate(itemToUpdate);

        if (validation.TryCheckValidation() == false)
            return Result<ToDoItemDto>.Failure(Error.ValidationError);

        // 5) Сохраняем и проверяем результат
        Result<ToDoItem> updatedItemResult = await _toDoRepository.UpdateAsync(itemToUpdate, cancellationToken);

        if (updatedItemResult.IsFailure)
            return Result<ToDoItemDto>.Failure(updatedItemResult.Error);

        // 6) Публикуем в кафке
        ToDoItemUpdatedTopic updatedEvent = new
        (
            TopicId: Guid.NewGuid(),

            itemToUpdate.Id, itemToUpdate.Title,
            itemToUpdate.AuditInfo.CreatedAt, itemToUpdate.AuditInfo.UpdatedAt,
            itemToUpdate.ScheduleInfo.DueDate, itemToUpdate.ScheduleInfo.DueTime,
            itemToUpdate.ScheduleInfo.TimeZoneId, itemToUpdate.ScheduleInfo.ScheduledAt,
            itemToUpdate.CompletionInfo.IsCompleted, itemToUpdate.CompletionInfo.FirstCompletedAt, itemToUpdate.CompletionInfo.LastCompletedAt,

            TopicUpdatedAt: SystemClock.Instance.GetCurrentInstant()
        );

        Result<ToDoItemUpdatedTopic> publishResult = await _publisher.PublishAsync(updatedEvent, cancellationToken);

        if (publishResult.IsFailure)
        {
            Log.Error($"Ошибка во время публикации сообщения в брокер сообщений! Ошибка: {publishResult.Error.Code}");
            return Result<ToDoItemDto>.Failure(Error.PublishError);
        }

        // 7) Маппим и отдаём DTO
        return Result<ToDoItemDto>.Success(_toDoMapper.MapToDto(updatedItemResult.Value));
    }

    public async Task<Result<ToDoItemDto>> DeleteAsync(Guid id, CancellationToken cancelToken)
    {
        Result<ToDoItem> deletedResult = await _toDoRepository.DeleteAsync(id, cancelToken);

        if (deletedResult.IsFailure)
            return Result<ToDoItemDto>.Failure(deletedResult.Error);

        ToDoItem deletedItem = deletedResult.Value;

        ToDoItemDeletedTopic deletedEvent = new
        (
            TopicId: Guid.NewGuid(),

            deletedItem.Id, deletedItem.Title,
            deletedItem.AuditInfo.CreatedAt, deletedItem.AuditInfo.UpdatedAt,
            deletedItem.ScheduleInfo.DueDate, deletedItem.ScheduleInfo.DueTime,
            deletedItem.ScheduleInfo.TimeZoneId, deletedItem.ScheduleInfo.ScheduledAt,
            deletedItem.CompletionInfo.IsCompleted, deletedItem.CompletionInfo.FirstCompletedAt, deletedItem.CompletionInfo.LastCompletedAt,

            TopicDeletedAt: SystemClock.Instance.GetCurrentInstant()
        );

        Result<ToDoItemDeletedTopic> publishResult = await _publisher.PublishAsync(deletedEvent, cancelToken);

        if (publishResult.IsFailure)
        {
            Log.Error($"Ошибка во время публикации сообщения в брокер сообщений! Ошибка: {publishResult.Error.Code}");
            return Result<ToDoItemDto>.Failure(Error.PublishError);
        }

        return Result<ToDoItemDto>.Success(_toDoMapper.MapToDto(deletedItem));
    }
}