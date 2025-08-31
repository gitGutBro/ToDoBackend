using ToDoBackend.Dtos;
using ToDoBackend.Models.ToDoItem;

namespace ToDoBackend.Mappers;

public class UpdateToDoMapper : IMapper<ToDoItem, UpdateToDoItemDto>
{
    public ToDoItem MapToModel(UpdateToDoItemDto dto)
    {
        ToDoItem item = new(dto.Title, dto.Description);

        if (dto.IsCompleted)
            item.MarkAsCompleted();
        else
            item.MarkAsUncompleted();

        item.SetScheduledInfo(dto.LocalDate + dto.LocalTime, dto.PreserveScheduleAtInstance, dto.TimeZoneId);

        return item;
    }

    public UpdateToDoItemDto MapToDto(ToDoItem mappable)
    {
        return new UpdateToDoItemDto(mappable.Title, mappable.Description!, 
            mappable.CompletionInfo.IsCompleted, mappable.ScheduleInfo.DueDate, 
            mappable.ScheduleInfo.DueTime, mappable.ScheduleInfo.TimeZoneId, true);
    }

    public void UpdateModel(ToDoItem item, UpdateToDoItemDto dto)
    {
        item.UpdateTitle(dto.Title);
        item.UpdateDescription(dto.Description);
        item.SetScheduledInfo(dto.LocalDate + dto.LocalTime, dto.PreserveScheduleAtInstance, dto.TimeZoneId);

        if (dto.IsCompleted)
            item.MarkAsCompleted();
        else
            item.MarkAsUncompleted();
    }
}