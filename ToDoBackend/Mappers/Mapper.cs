namespace ToDoBackend.Utils;

public abstract class Mapper<TMappable, TDto> where TMappable : IMappable
{
    public abstract TMappable Map(TDto dto);
    public abstract TDto Map(TMappable mappable);
}