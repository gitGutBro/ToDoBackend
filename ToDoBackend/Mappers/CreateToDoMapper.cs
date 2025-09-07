using Shared.Mappers;
using ToDoBackend.Dtos;
using ToDoBackend.Entities.ToDoItem;

namespace ToDoBackend.Mappers;

internal class CreateToDoMapper : IMapper<ToDoItem, CreateToDoItemDto>
{
    public ToDoItem MapToModel(CreateToDoItemDto dto) =>
        new(dto.Title!, dto.Description);

    public CreateToDoItemDto MapToDto(ToDoItem mappable) =>
        new(mappable.Title, mappable.Description);
}