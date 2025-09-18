using NodaTime;

namespace Domain.Topics;

public sealed record class ToDoItemUpdatedTopic
(
    Guid TopicId,
    Guid EntityId, string Title,
    Instant CreatedAt, Instant? UpdatedAt,
    LocalDate? DueDate, LocalTime? DueTime,
    string? TimeZoneId, Instant? ScheduledAt,
    bool IsCompleted, Instant? FirstCompletedAt, Instant? LastCompletedAt,
    Instant TopicUpdatedAt
);