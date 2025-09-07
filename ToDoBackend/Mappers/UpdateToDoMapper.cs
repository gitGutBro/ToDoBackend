using Shared.Mappers;
using ToDoBackend.Dtos;
using ToDoBackend.Entities.ToDoItem;

namespace ToDoBackend.Mappers;

internal class UpdateToDoMapper : IUpdateMapper<ToDoItem, UpdateToDoItemDto>
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