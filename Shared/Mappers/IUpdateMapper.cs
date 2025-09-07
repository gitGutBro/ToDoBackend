using Shared.Entities;

namespace Shared.Mappers;

public interface IUpdateMapper<TModel, TDto> where TModel : IEntity
{
    void UpdateModel(TModel item, TDto dto);
}