using ToDoBackend.Dtos;
using ToDoBackend.Models;

namespace ToDoBackend.Utils;

public class UpdateToDoMapper : Mapper<ToDoItem, UpdateToDoItemDto>
{
    public override ToDoItem Map(UpdateToDoItemDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "DTO не может быть null.");

        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("Заголовок задачи не может быть пустым.", nameof(dto.Title));

        return new ToDoItem(dto.Title)
        {
            IsCompleted = dto.IsCompleted
        };
    }

    public override UpdateToDoItemDto Map(ToDoItem mappable)
    {
        if (mappable == null)
            throw new ArgumentNullException(nameof(mappable), "Маппируемый объект не может быть null.");

        if (string.IsNullOrWhiteSpace(mappable.Title))
            throw new ArgumentException("Заголовок задачи не может быть пустым.", nameof(mappable.Title));

        return new UpdateToDoItemDto(mappable.Title, mappable.IsCompleted);
    }

    public void Update(ToDoItem item, UpdateToDoItemDto dto)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "Маппируемый объект не может быть null.");

        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "DTO не может быть null.");

        item.UpdateTitle(dto.Title);
        item.IsCompleted = dto.IsCompleted;
    }
}