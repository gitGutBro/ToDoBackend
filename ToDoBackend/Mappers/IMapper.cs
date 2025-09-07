using ToDoBackend.Models;

namespace ToDoBackend.Mappers;

public interface IMapper<TMappable, TDto> where TMappable : IModel
{
    TMappable MapToModel(TDto dto);
    TDto MapToDto(TMappable mappable);
}