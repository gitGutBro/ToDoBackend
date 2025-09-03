using ToDoBackend.Models;

namespace ToDoBackend.Mappers;

public interface IMapper<TMappable, TDto> where TMappable : IModel
{
    public abstract TMappable MapToModel(TDto dto);
    public abstract TDto MapToDto(TMappable mappable);
}