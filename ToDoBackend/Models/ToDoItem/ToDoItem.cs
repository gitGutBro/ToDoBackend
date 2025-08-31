using NodaTime;
using Serilog;

namespace ToDoBackend.Models.ToDoItem;

public class ToDoItem : IModel, IDisposable
{
    public ToDoItem(string title, string? description = null)
    {
        Title = new Title(title.Trim());
        Description = description == null ? "" : description.Trim();
        Changed += AuditInfo.RecordUpdate;
    }

    public event Action? Changed;

    public Guid Id { get; } = Guid.NewGuid();
    public Title Title { get; }
    public string? Description { get; private set; }
    public AuditInfo AuditInfo { get; } = new();
    public ScheduleInfo ScheduleInfo { get; } = new();
    public CompletionInfo CompletionInfo { get; } = new();
    public uint? Xmin { get; private set; }

    public void UpdateTitle(string newTitle)
    {
        Title.SetValue(newTitle);
        Changed?.Invoke();
    }

    public void UpdateDescription(string? newDescription)
    {
        Description = newDescription?.Trim() ?? "";
        Changed?.Invoke();
    }

    public void SetScheduledInfo(LocalDateTime? dueDateTime, bool preserveInstance, string? timeZoneId = null)
    {
        bool isChanged = false;

        if (dueDateTime!.Value.Date != ScheduleInfo.DueDate)
        {
            ScheduleInfo.SetDueDate(dueDateTime!.Value.Date);
            isChanged = true;
        }

        if (dueDateTime!.Value.TimeOfDay != ScheduleInfo.DueTime)
        {
            ScheduleInfo.SetDueTime(dueDateTime!.Value.TimeOfDay);
            isChanged = true;
        }

        if (timeZoneId != null)
        {
            if (ScheduleInfo.TimeZoneId == timeZoneId)
                return;

            DateTimeZone? newTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneId);

            if (newTimeZone == null)
                Log.Error($"Invalid time zone ID: {timeZoneId}");

            ScheduleInfo.SetTimeZoneId(timeZoneId, preserveInstance);
        }

        if (isChanged)
            Changed?.Invoke();
    }

    public void SetDueTime(LocalTime dueTime)
    {
        ScheduleInfo.SetDueTime(dueTime);
        Changed?.Invoke();
    }

    public void SetDueDate(LocalDate dueDate)
    {
        ScheduleInfo.SetDueDate(dueDate);
        Changed?.Invoke();
    }

    public void SetTimeZone(string timeZoneId, bool preserveInstant = false)
    {
        ScheduleInfo.SetTimeZoneId(timeZoneId, preserveInstant);
        Changed?.Invoke();
    }

    public void MarkAsCompleted()
    {
        CompletionInfo.MarkAsCompleted();
        Changed?.Invoke();
    }

    public void MarkAsUncompleted()
    {
        CompletionInfo.MarkAsUncompleted();
        Changed?.Invoke();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Changed -= AuditInfo.RecordUpdate;
    }
}