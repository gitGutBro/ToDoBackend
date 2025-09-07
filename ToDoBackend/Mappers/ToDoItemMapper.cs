using Shared.Mappers;
using ToDoBackend.Dtos;
using ToDoBackend.Entities.ToDoItem;

namespace ToDoBackend.Mappers;

internal class ToDoItemMapper : IMapper<ToDoItem, ToDoItemDto>
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