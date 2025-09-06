using NodaTime;
using NodaTime.TimeZones;
using Serilog;

namespace ToDoBackend.Models.ToDoItem;

public record class ScheduleInfo
{
    public LocalDate? DueDate { get; private set; }
    public LocalTime? DueTime { get; private set; }
    public string? TimeZoneId { get; private set; }
    public Instant? ScheduledAt { get; private set; }

    public bool TrySetDueDate(LocalDate? dueDate, bool preserveInstant = false)
    {
        if (DueDate == dueDate)
        {
            Log.Warning("Значение не изменилось.");
            return false;
        }

        DueDate = dueDate;

        if (preserveInstant == false)
            SetScheduledAt();

        return true;
    }

    public bool TrySetDueTime(LocalTime? dueTime, bool preserveInstant = false)
    {
        if (DueTime == dueTime)
        {
            Log.Warning("Значение не изменилось.");
            return false;
        }

        DueTime = dueTime;

        if (preserveInstant == false)
            SetScheduledAt();

        return true;
    }

    public bool TrySetTimeZoneId(string timeZoneId, bool preserveInstant = false)
    {
        if (string.IsNullOrWhiteSpace(timeZoneId))
            throw new ArgumentNullException(nameof(timeZoneId), "TimeZoneId cannot be null or empty.");

        if (timeZoneId == TimeZoneId)
        {
            Log.Warning("Значение не изменилось.");
            return false;
        }    

        DateTimeZone? newTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneId)
            ?? throw new ArgumentException($"Invalid time zone ID: {timeZoneId}", nameof(timeZoneId));

        if (preserveInstant && ScheduledAt.HasValue)
        {
            ZonedDateTime zonedDateTime = ScheduledAt.Value.InZone(newTimeZone);
            TimeZoneId = timeZoneId;
            DueDate = zonedDateTime.Date;
            DueTime = zonedDateTime.TimeOfDay;
            return true;
        }

        TimeZoneId = timeZoneId;
        SetScheduledAt();
        return true;
    }

    private void SetScheduledAt()
    {
        bool canSetNotNull = DueDate.HasValue && DueTime.HasValue && string.IsNullOrWhiteSpace(TimeZoneId) == false;

        if (canSetNotNull)
        {
            DateTimeZone? timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(TimeZoneId!);

            if (timeZone == null)
            {
                ScheduledAt = null;
                return;
            }

            ScheduledAt = CalculateScheduledAt(DueDate!.Value, DueTime!.Value, timeZone);
        }
        else
        {
            ScheduledAt = null;
        }
    }

    private static Instant CalculateScheduledAt(LocalDate localDate, LocalTime localTime, DateTimeZone timeZone)
    {
        LocalDateTime localDateTime = localDate + localTime;
        ZonedDateTime zoned = timeZone.ResolveLocal(localDateTime, Resolvers.LenientResolver); //LenientResolver игнорирует пропуски и неоднозначности во времени.Если нужно их учитывать, нужно использовать Resolvers.StrictResolver
        return zoned.ToInstant();
    }
}