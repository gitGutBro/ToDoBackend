using ApplicationBackend.Dtos;
using Domain.Entities.ToDoItem;

namespace ApplicationBackend.Mappers;

public class ToDoItemMapper : IMapper<ToDoItem, ToDoItemDto>
{
    public ToDoItem MapToModel(ToDoItemDto dto)
    {
        ToDoItem item = new(dto.Title!, dto.Description);

        if (dto.IsCompleted)
            item.MarkAsCompleted();
        else
            item.MarkAsUncompleted();

        item.SetScheduledInfo(dto.DueDate + dto.DueTime, preserveInstant: false, dto.TimeZoneId);

        return item;
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