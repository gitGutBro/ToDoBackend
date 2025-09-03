using NodaTime;
using System.Text.Json.Serialization;

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
    [JsonIgnore] public uint? Xmin { get; private set; }

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

    public void SetScheduledInfo(LocalDateTime? dueDateTime, bool preserveInstant, string? timeZoneId = null)
    {
        bool isChanged = false;

        if (string.IsNullOrWhiteSpace(timeZoneId) == false && timeZoneId != ScheduleInfo.TimeZoneId)
        {
            ScheduleInfo.SetTimeZoneId(timeZoneId, preserveInstant);
            isChanged = true;
        }

        if (dueDateTime.HasValue)
        {
            LocalDateTime localDateTime = dueDateTime.Value;

            if (ScheduleInfo.DueDate != localDateTime.Date)
            {
                ScheduleInfo.SetDueDate(localDateTime.Date, preserveInstant);
                isChanged = true;
            }

            if (ScheduleInfo.DueTime != localDateTime.TimeOfDay)
            {
                ScheduleInfo.SetDueTime(localDateTime.TimeOfDay, preserveInstant);
                isChanged = true;
            }
        }

        if (isChanged)
            Changed?.Invoke();
    }

    public void SetDueTime(LocalTime dueTime, bool preserveInstant = false)
    {
        ScheduleInfo.SetDueTime(dueTime, preserveInstant);
        Changed?.Invoke();
    }

    public void SetDueDate(LocalDate dueDate, bool preserveInstant = false)
    {
        ScheduleInfo.SetDueDate(dueDate, preserveInstant);
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