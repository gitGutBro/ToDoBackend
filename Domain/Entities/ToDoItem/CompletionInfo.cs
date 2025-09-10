using NodaTime;

namespace Domain.Entities.ToDoItem;

public record class CompletionInfo
{
    public bool IsCompleted { get; set; }
    public Instant? FirstCompletedAt { get; set; }
    public Instant? LastCompletedAt { get; set; }
}