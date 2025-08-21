using ToDoBackend.Dtos;
using ToDoBackend.Models;

namespace ToDoBackend.Mappers;

public class UpdateToDoMapper : IMapper<ToDoItem, UpdateToDoItemDto>
{
    public ToDoItem MapToModel(UpdateToDoItemDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto), "DTO не может быть null.");

        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("Заголовок задачи не может быть пустым.", nameof(dto));

        return new ToDoItem(dto.Title)
        {
            IsCompleted = dto.IsCompleted
        };
    }

    public UpdateToDoItemDto MapToDto(ToDoItem mappable)
    {
        if (mappable is null)
            throw new ArgumentNullException(nameof(mappable), "Маппируемый объект не может быть null.");

        if (string.IsNullOrWhiteSpace(mappable.Title))
            throw new ArgumentException("Заголовок задачи не может быть пустым.", nameof(mappable));

        return new UpdateToDoItemDto(mappable.Title, mappable.IsCompleted);
    }

    public void UpdateModel(ToDoItem item, UpdateToDoItemDto dto)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "Маппируемый объект не может быть null.");

        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "DTO не может быть null.");

        item.UpdateTitle(dto.Title);
        item.IsCompleted = dto.IsCompleted;
    }
}