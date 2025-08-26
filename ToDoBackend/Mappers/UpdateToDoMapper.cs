using ToDoBackend.Dtos;
using ToDoBackend.Models;

namespace ToDoBackend.Mappers;

public class UpdateToDoMapper : IMapper<ToDoItem, UpdateToDoItemDto>
{
    public ToDoItem MapToModel(UpdateToDoItemDto dto) => 
        new(dto.Title)
        {
            IsCompleted = dto.IsCompleted
        };

    public UpdateToDoItemDto MapToDto(ToDoItem mappable) =>
        new(mappable.Title, mappable.IsCompleted);

    public void UpdateModel(ToDoItem item, UpdateToDoItemDto dto)
    {
        item.UpdateTitle(dto.Title);
        item.IsCompleted = dto.IsCompleted;
    }
}