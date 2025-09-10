using Domain.Entities.ToDoItem;
using Shared.ResultPattern;

namespace Domain.Services;

public interface ICompletionService
{
    Result<bool> TryMarkAsCompleted(CompletionInfo completionInfo);
    Result<bool> TryMarkAsUncompleted(CompletionInfo completionInfo);
}