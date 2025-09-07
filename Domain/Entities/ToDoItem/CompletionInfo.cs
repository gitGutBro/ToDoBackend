using NodaTime;

namespace Domain.Entities.ToDoItem;

public record class CompletionInfo
{
    public bool IsCompleted { get; private set; }
    public Instant? FirstCompletedAt { get; private set; }
    public Instant? LastCompletedAt { get; private set; }

    public bool TryMarkAsCompleted()
    {
        if (IsCompleted)
            return false;

        Instant now = SystemClock.Instance.GetCurrentInstant();

        if (FirstCompletedAt == null)
            FirstCompletedAt = now;

        LastCompletedAt = now;
        IsCompleted = true;
        return true;
    }

    public bool TryMarkAsUncompleted()
    {
        if (IsCompleted == false)
            return false;

        IsCompleted = false;
        return true;
    }
}