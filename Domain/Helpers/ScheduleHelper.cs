using NodaTime;
using NodaTime.TimeZones;
using Domain.Entities.ToDoItem;
using Domain.Specifications.Schedule;
using Shared.ResultPattern;

namespace Domain.Helpers;

internal static class ScheduleHelper
{
    private static readonly TimeZoneIdSpecification _timeZoneIdSpecification = new();

    public static Result<bool> TrySetDueDate(ScheduleInfo scheduleInfo, LocalDate? dueDate, bool preserveInstant = false) => 
        TrySetValue(scheduleInfo, dueDate, scheduleInfo.DueDate,
                    value => scheduleInfo.DueDate = value,
                    preserveInstant);

    public static Result<bool> TrySetDueTime(ScheduleInfo scheduleInfo, LocalTime? dueTime, bool preserveInstant = false) => 
        TrySetValue(scheduleInfo, dueTime, scheduleInfo.DueTime,
                    value => scheduleInfo.DueTime = value,
                    preserveInstant);

    public static Result<bool> TrySetTimeZoneId(ScheduleInfo scheduleInfo, string timeZoneId, bool preserveInstant = false)
    {
        if (timeZoneId == scheduleInfo.TimeZoneId)
            return Result<bool>.Success(false);

        if (_timeZoneIdSpecification.TryGetZone(timeZoneId, out DateTimeZone? zone, out Error error) == false)
            return Result<bool>.Failure(error);

        if (preserveInstant && scheduleInfo.ScheduledAt.HasValue)
        {
            ZonedDateTime zonedDateTime = scheduleInfo.ScheduledAt.Value.InZone(zone!);
            scheduleInfo.TimeZoneId = timeZoneId;
            scheduleInfo.DueDate = zonedDateTime.Date;
            scheduleInfo.DueTime = zonedDateTime.TimeOfDay;
            return Result<bool>.Success(true);
        }

        scheduleInfo.TimeZoneId = timeZoneId;
        SetScheduledAt(scheduleInfo);
        return Result<bool>.Success(true);
    }

    public static Result<bool> TrySetScheduledInfo(ScheduleInfo scheduleInfo, LocalDateTime? dueDateTime, bool preserveInstant, string? timeZoneId = null)
    {
        bool hasChanges = false;

        if (string.IsNullOrWhiteSpace(timeZoneId) == false)
        {
            Result<bool> timeZoneResult = TrySetTimeZoneId(scheduleInfo, timeZoneId, preserveInstant);

            if (timeZoneResult.IsFailure)
                return timeZoneResult;

            hasChanges |= timeZoneResult.Value;
        }

        if (dueDateTime.HasValue)
        {
            LocalDateTime localDateTime = dueDateTime.Value;

            Result<bool> dateResult = TrySetDueDate(scheduleInfo, localDateTime.Date, preserveInstant);

            if (dateResult.IsFailure)
                return dateResult;

            hasChanges |= dateResult.Value;

            Result<bool> timeResult = TrySetDueTime(scheduleInfo, localDateTime.TimeOfDay, preserveInstant);

            if (timeResult.IsFailure)
                return timeResult;

            hasChanges |= timeResult.Value;
        }

        return Result<bool>.Success(hasChanges);
    }

    private static void SetScheduledAt(ScheduleInfo scheduleInfo)
    {
        bool canSetNotNull = scheduleInfo.DueDate.HasValue && 
                           scheduleInfo.DueTime.HasValue && 
                           string.IsNullOrWhiteSpace(scheduleInfo.TimeZoneId) == false;

        if (canSetNotNull)
        {
            DateTimeZone? timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(scheduleInfo.TimeZoneId!);

            if (timeZone == null)
            {
                scheduleInfo.ScheduledAt = null;
                return;
            }

            scheduleInfo.ScheduledAt = CalculateScheduledAt(scheduleInfo.DueDate!.Value, scheduleInfo.DueTime!.Value, timeZone);
        }
        else
        {
            scheduleInfo.ScheduledAt = null;
        }
    }

    private static Instant CalculateScheduledAt(LocalDate localDate, LocalTime localTime, DateTimeZone timeZone)
    {
        LocalDateTime localDateTime = localDate + localTime;
        ZonedDateTime zoned = timeZone.ResolveLocal(localDateTime, Resolvers.LenientResolver);
        return zoned.ToInstant();
    }

    private static Result<bool> TrySetValue<TEqualValue>
    (
        ScheduleInfo scheduleInfo,
        TEqualValue newValue,
        TEqualValue currentValue,
        Action<TEqualValue> setValue,
        bool preserveInstant
    )
    {
        if (EqualityComparer<TEqualValue>.Default.Equals(newValue, currentValue))
            return Result<bool>.Success(false);

        setValue(newValue);

        if (preserveInstant == false)
            SetScheduledAt(scheduleInfo);

        return Result<bool>.Success(true);
    }
}