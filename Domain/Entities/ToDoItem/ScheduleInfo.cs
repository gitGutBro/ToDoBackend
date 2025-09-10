using NodaTime;

namespace Domain.Entities.ToDoItem;

public record class ScheduleInfo
{
    public LocalDate? DueDate { get; set; }
    public LocalTime? DueTime { get; set; }
    public string? TimeZoneId { get; set; }
    public Instant? ScheduledAt { get; set; }
}