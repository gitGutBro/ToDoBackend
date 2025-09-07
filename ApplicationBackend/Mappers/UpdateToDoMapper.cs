using ApplicationBackend.Dtos;
using Domain.Entities.ToDoItem;

namespace ApplicationBackend.Mappers;

public class UpdateToDoMapper : IUpdateMapper<ToDoItem, UpdateToDoItemDto>
{
    public void UpdateModel(ToDoItem item, UpdateToDoItemDto dto)
    {
        item.UpdateTitle(dto.Title);
        item.UpdateDescription(dto.Description);
        item.SetScheduledInfo(dto.LocalDate + dto.LocalTime, preserveInstant: false, dto.TimeZoneId);

        if (dto.IsCompleted)
            item.MarkAsCompleted();
        else
            item.MarkAsUncompleted();
    }
}