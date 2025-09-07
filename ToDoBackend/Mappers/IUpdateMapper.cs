using ToDoBackend.Models;

namespace ToDoBackend.Mappers;

public interface IUpdateMapper<TModel, TDto> where TModel : IModel
{
    void UpdateModel(TModel item, TDto dto);
}