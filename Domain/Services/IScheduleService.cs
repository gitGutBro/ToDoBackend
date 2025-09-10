using Domain.Entities.ToDoItem;
using NodaTime;
using Shared.ResultPattern;

namespace Domain.Services;

public interface IScheduleService
{
    Result<bool> TrySetDueDate(ScheduleInfo scheduleInfo, LocalDate? dueDate, bool preserveInstant = false);
    Result<bool> TrySetDueTime(ScheduleInfo scheduleInfo, LocalTime? dueTime, bool preserveInstant = false);
    Result<bool> TrySetTimeZoneId(ScheduleInfo scheduleInfo, string timeZoneId, bool preserveInstant = false);
    Result<bool> TrySetScheduledInfo(ScheduleInfo scheduleInfo, LocalDateTime? dueDateTime, bool preserveInstant, string? timeZoneId = null);
}