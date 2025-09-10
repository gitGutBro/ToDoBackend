using Serilog;
using Shared.ResultPattern;

namespace Shared.Extensions;

public static class ResultExtensions
{
    public static Result<bool> HandleEntityChange
    (
        this Result<bool> result,
        Action Changed,
        string operationName,
        Guid entityId)
    {
        return result.Match
        (
            success: isChanged =>
            {
                if (isChanged)
                    Changed?.Invoke();

                return Result<bool>.Success(true);
            },
            failure: error =>
            {
                Log.Error("Failed to {Operation} for ToDoItem {ToDoItemId}. Error: {Error}",
                         operationName, entityId, error);

                return Result<bool>.Failure(error);
            }
        );
    }
}