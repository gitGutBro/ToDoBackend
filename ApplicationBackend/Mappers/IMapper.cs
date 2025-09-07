using Domain.Entities;

namespace ApplicationBackend.Mappers;

public interface IMapper<TMappable, TDto> where TMappable : IEntity
{
    TMappable MapToModel(TDto dto);
    TDto MapToDto(TMappable mappable);
}