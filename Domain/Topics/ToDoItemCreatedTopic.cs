using NodaTime;

namespace Domain.Topics;

public sealed record class ToDoItemCreatedTopic
(
    Guid TopicId,
    Guid EntityId, string Title,
    Instant CreatedAt,
    LocalDate? DueDate, LocalTime? DueTime,
    string? TimeZoneId, Instant? ScheduledAt,
    Instant TopicCreatedAt
);