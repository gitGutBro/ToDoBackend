using NodaTime;
using NodaTime.TimeZones;

namespace ToDoBackend.Models.ToDoItem;

public record class ScheduleInfo
{
    public LocalDate? DueDate { get; private set; }
    public LocalTime? DueTime { get; private set; }
    public string? TimeZoneId { get; private set; }
    public Instant? ScheduledAt { get; private set; }

    public void SetDueDate(LocalDate? dueDate)
    {
        DueDate = dueDate;
        SetScheduledAt();
    }

    public void SetDueTime(LocalTime? dueTime)
    {
        DueTime = dueTime;
        SetScheduledAt();
    }

    public void SetTimeZoneId(string timeZoneId, bool preserveInstant = false)
    {
        if (string.IsNullOrWhiteSpace(timeZoneId))
            throw new ArgumentNullException(nameof(timeZoneId), "TimeZoneId cannot be null or empty.");

        if (timeZoneId == TimeZoneId)
            return;

        DateTimeZone? newTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneId)
            ?? throw new ArgumentException($"Invalid time zone ID: {timeZoneId}", nameof(timeZoneId));

        TimeZoneId = timeZoneId;

        if (preserveInstant)
        {
            if (ScheduledAt.HasValue == false)
                return;

            ZonedDateTime zoneDateTime = ScheduledAt.Value.InZone(newTimeZone);
            DueDate = zoneDateTime.Date;
            DueTime = zoneDateTime.TimeOfDay;
        }
        else
        {
            SetScheduledAt();
        }
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