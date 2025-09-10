using ApplicationBackend.Dtos;
using Domain.Entities.ToDoItem;
using Domain.Factories;
using Shared.ResultPattern;

namespace ApplicationBackend.Mappers;

public class ToDoItemMapper(IToDoItemFactory toDoItemFactory) : IMapper<ToDoItem, ToDoItemDto>
{
    private readonly IToDoItemFactory _toDoItemFactory = toDoItemFactory;

    public Result<ToDoItem> MapToModel(ToDoItemDto dto)
    {
        ToDoItem item = _toDoItemFactory.Create(dto.Title!, dto.Description);

        Result<bool> resultCompletion = dto.IsCompleted
            ? item.MarkAsCompleted()
            : item.MarkAsUncompleted();

        if (resultCompletion.IsFailure)
            return Result<ToDoItem>.Failure(resultCompletion.Error);

        Result<bool> resultSchedule = item.SetScheduledInfo(dto.DueDate + dto.DueTime, dto.PreserveInstant, dto.TimeZoneId);

        if (resultSchedule.IsFailure)
            return Result<ToDoItem>.Failure(resultSchedule.Error);

        return Result<ToDoItem>.Success(item);
    }

    public ToDoItemDto MapToDto(ToDoItem mappable) =>
        new
        (
            mappable.Id,
            mappable.Title,
            mappable.Description,
            mappable.CompletionInfo.IsCompleted,
            mappable.ScheduleInfo.DueDate,
            mappable.ScheduleInfo.DueTime,
            mappable.ScheduleInfo.TimeZoneId
        );
}