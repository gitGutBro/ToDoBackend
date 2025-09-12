using NodaTime;

namespace Domain.Entities.ToDoItem;

public record class ScheduleInfo
{
    public LocalDate? DueDate { get; internal set; }
    public LocalTime? DueTime { get; internal set; }
    public string? TimeZoneId { get; internal set; }
    public Instant? ScheduledAt { get; internal set; }
}