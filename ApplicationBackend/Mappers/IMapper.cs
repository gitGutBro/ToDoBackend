using Domain.Entities;
using Shared.ResultPattern;

namespace ApplicationBackend.Mappers;

public interface IMapper<TMappable, TDto> where TMappable : IEntity
{
    Result<TMappable> MapToModel(TDto dto);
    TDto MapToDto(TMappable mappable);
}