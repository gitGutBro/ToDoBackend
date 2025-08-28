using Microsoft.AspNetCore.Mvc;
using ToDoBackend.ResultPattern;

namespace ToDoBackend.Extensions;

public static class BaseControllerExtensions
{
    /// <summary>
    /// Выполнить асинхронное действие (возвращающее Result{T}), обработать отмену и маппинг ошибок.
    /// </summary>
    public static async Task<IActionResult> ExecuteAsync<TValue>(
        this ControllerBase controller,
        Func<CancellationToken, Task<Result<TValue>>> action,
        Func<TValue, IActionResult> onSuccess,
        CancellationToken cancelToken)
    {
        ArgumentNullException.ThrowIfNull(controller);
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(onSuccess);

        if (cancelToken.IsCancellationRequested)
            return controller.StatusCode(499, "Request was cancelled.");

        try
        {
            Result<TValue> result = await action(cancelToken);

            return result.Match(
                value => onSuccess(value),
                error => error.ToActionResult()
            );
        }
        catch (OperationCanceledException)
        {
            return controller.StatusCode(499, "Request was cancelled.");
        }
    }

    /// <summary>
    /// Перегрузка ExecuteAsync для действий, когда нас интересует только факт успеха (не значение).
    /// onSuccess вызывается при успешном результате и должен вернуть соответствующий IActionResult (например NoContent()).
    /// </summary>
    public static async Task<IActionResult> ExecuteAsync<T>(
        this ControllerBase controller,
        Func<CancellationToken, Task<Result<T>>> action,
        Func<IActionResult> onSuccess,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(controller);
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(onSuccess);

        if (cancellationToken.IsCancellationRequested)
            return controller.StatusCode(499, "Request was cancelled.");

        try
        {
            Result<T> result = await action(cancellationToken);

            return result.Match(
                value => onSuccess(),
                error => error.ToActionResult()
            );
        }
        catch (OperationCanceledException)
        {
            return controller.StatusCode(499, "Request was cancelled.");
        }
    }
}