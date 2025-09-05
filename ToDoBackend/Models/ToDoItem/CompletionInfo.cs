using NodaTime;

namespace ToDoBackend.Models.ToDoItem;

public record class CompletionInfo
{
    public bool IsCompleted { get; private set; }
    public Instant? FirstCompletedAt { get; private set; }
    public Instant? LastCompletedAt { get; private set; }

    public void MarkAsCompleted()
    {
        if (IsCompleted)
            return;

        Instant now = SystemClock.Instance.GetCurrentInstant();

        if (FirstCompletedAt == null)
            FirstCompletedAt = now;

        LastCompletedAt = now;
        IsCompleted = true;
    }

    public void MarkAsUncompleted() => 
        IsCompleted = false;
}