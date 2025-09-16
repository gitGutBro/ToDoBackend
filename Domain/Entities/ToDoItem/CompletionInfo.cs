using NodaTime;

namespace Domain.Entities.ToDoItem;

public record class CompletionInfo
{
    public bool IsCompleted { get; internal set; }
    public Instant? FirstCompletedAt { get; internal set; }
    public Instant? LastCompletedAt { get; internal set; }
}