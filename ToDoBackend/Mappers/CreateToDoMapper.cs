using ToDoBackend.Dtos;
using ToDoBackend.Models;

namespace ToDoBackend.Mappers;

public class CreateToDoMapper : IMapper<ToDoItem, CreateToDoItemDto>
{
    public ToDoItem MapToModel(CreateToDoItemDto dto) => 
        new(dto.Title!)
        {
            IsCompleted = dto.IsCompleted
        };

    public CreateToDoItemDto MapToDto(ToDoItem mappable) =>
        new(mappable.Title, mappable.IsCompleted);
}