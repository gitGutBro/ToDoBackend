using NodaTime;
using Domain.Entities.ToDoItem;
using Shared.ResultPattern;

namespace Domain.Helpers;

internal static class CompletionHelper
{
    public static Result<bool> TryMarkAsCompleted(CompletionInfo completionInfo)
    {
        if (completionInfo.IsCompleted)
            return Result<bool>.Success(false);

        Instant now = SystemClock.Instance.GetCurrentInstant();

        if (completionInfo.FirstCompletedAt == null)
            completionInfo.FirstCompletedAt = now;

        completionInfo.LastCompletedAt = now;
        completionInfo.IsCompleted = true;
        return Result<bool>.Success(true);
    }

    public static Result<bool> TryMarkAsUncompleted(CompletionInfo completionInfo)
    {
        if (completionInfo.IsCompleted == false)
            return Result<bool>.Success(false);

        completionInfo.IsCompleted = false;
        return Result<bool>.Success(true);
    }
}

