using Domain.Entities;
using Shared.ResultPattern;

namespace ApplicationBackend.Mappers;

public interface IUpdateMapper<TModel, TDto> where TModel : IEntity
{
    Result<bool> UpdateEntity(TModel item, TDto dto);
}