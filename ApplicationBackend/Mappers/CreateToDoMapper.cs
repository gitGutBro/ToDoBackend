using ApplicationBackend.Dtos;
using Domain.Entities.ToDoItem;

namespace ApplicationBackend.Mappers;

public class CreateToDoMapper : IMapper<ToDoItem, CreateToDoItemDto>
{
    public ToDoItem MapToModel(CreateToDoItemDto dto) =>
        new(dto.Title!, dto.Description);

    public CreateToDoItemDto MapToDto(ToDoItem mappable) =>
        new(mappable.Title, mappable.Description);
}