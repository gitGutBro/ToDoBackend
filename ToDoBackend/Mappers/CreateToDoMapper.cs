using ToDoBackend.Dtos;
using ToDoBackend.Models;

namespace ToDoBackend.Mappers;

public class CreateToDoMapper : Mapper<ToDoItem, CreateToDoItemDto>
{
    public override ToDoItem Map(CreateToDoItemDto dto)
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

    public override CreateToDoItemDto Map(ToDoItem mappable)
    {
        if (mappable == null)
            throw new ArgumentNullException(nameof(mappable), "Маппируемый объект не может быть null.");

        if (string.IsNullOrWhiteSpace(mappable.Title))
            throw new ArgumentException("Заголовок задачи не может быть пустым.", nameof(mappable.Title));

        return new CreateToDoItemDto(mappable.Title, mappable.IsCompleted);
    }
}