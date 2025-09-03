using ToDoBackend.ResultPattern;

namespace ToDoBackend.Extensions;

public static class ResultExtensions
{
    public static T UnwrapOrThrow<T>(this Result<T> result)
    {
        return result.Match(
            success => success,
            failure => throw new InvalidOperationException($"Operation failed: {failure}")
        );
    }

    public static T? UnwrapOrDefault<T>(this Result<T?> result) where T : class
    {
        return result.Match(
            success => success,
            failure => default
        );
    }

    public static IEnumerable<T> UnwrapEnumerableOrEmpty<T>(this Result<IEnumerable<T>> result)
    {
        return result.Match(
            success => success ?? [],
            failure => []
        );
    }
}