using Shared.Entities;

namespace Shared.Mappers;

public interface IMapper<TMappable, TDto> where TMappable : IEntity
{
    TMappable MapToModel(TDto dto);
    TDto MapToDto(TMappable mappable);
}