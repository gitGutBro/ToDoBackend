using Microsoft.AspNetCore.Mvc;
using Shared.ResultPattern;

namespace Shared.Extensions;

public static class BaseControllerExtensions
{
    /// <summary>
    /// Выполнить асинхронное действие (возвращающее Result{T}), обработать отмену и маппинг ошибок.
    /// </summary>
    public static async Task<IActionResult> ExecuteAsync<TResult>
    (
        this ControllerBase controller,
        Func<CancellationToken, Task<Result<TResult>>> action,
        Func<TResult, IActionResult> onSuccess,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(controller);
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(onSuccess);

        try
        {
            Result<TResult> result = await action(cancellationToken);

            return result.Match
            (
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
    public static async Task<IActionResult> ExecuteAsyncNoResult<TValue>
    (
        this ControllerBase controller,
        Func<CancellationToken, Task<Result<TValue>>> action,
        Func<IActionResult> onSuccess,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(controller);
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(onSuccess);

        try
        {
            Result<TValue> result = await action(cancellationToken);

            return result.Match
            (
                _ => onSuccess(),
                error => error.ToActionResult()
            );
        }
        catch (OperationCanceledException)
        {
            return controller.StatusCode(499, "Request was cancelled.");
        }
    }
}