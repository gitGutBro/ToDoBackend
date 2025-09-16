using ApplicationBackend.Dtos;
using Domain.Entities.ToDoItem;
using Domain.Factories;
using Shared.ResultPattern;

namespace ApplicationBackend.Mappers;

public class CreateToDoMapper(IToDoItemFactory toDoItemFactory) : IMapper<ToDoItem, CreateToDoItemDto>
{
    private readonly IToDoItemFactory _toDoItemFactory = toDoItemFactory;

    public Result<ToDoItem> MapToModel(CreateToDoItemDto dto)
    {
        ToDoItem item = _toDoItemFactory.Create(dto.Title!, dto.Description);

        return Result<ToDoItem>.Success(item);
    }

    public CreateToDoItemDto MapToDto(ToDoItem mappable) =>
        new(mappable.Title, mappable.Description);
}