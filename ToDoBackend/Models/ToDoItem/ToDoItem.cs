using NodaTime;
using Serilog;
using System.Text.Json.Serialization;

namespace ToDoBackend.Models.ToDoItem;

public class ToDoItem : IModel, IDisposable
{
    [JsonIgnore] private uint? _xmin;

    public ToDoItem(string title, string? description = null)
    {
        Title = title.Trim();
        Description = description == null ? "" : description.Trim();
        Changed += AuditInfo.RecordUpdate;
    }

    public event Action? Changed;

    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public AuditInfo AuditInfo { get; } = new();
    public ScheduleInfo ScheduleInfo { get; } = new();
    public CompletionInfo CompletionInfo { get; } = new();

    public void UpdateTitle(string newTitle)
    {
        string trimmed = newTitle.Trim();

        if (Title == trimmed)
            return;

        Title = trimmed;
        Changed?.Invoke();
    }

    public void UpdateDescription(string? newDescription)
    {
        string trimmed = newDescription?.Trim() ?? "";

        if (trimmed == Description)
            return;

        Description = trimmed;
        Changed?.Invoke();
    }

    public void SetScheduledInfo(LocalDateTime? dueDateTime, bool preserveInstant, string? timeZoneId = null)
    {
        bool isChanged = false;

        if (string.IsNullOrWhiteSpace(timeZoneId))
            Log.Error("Таймзона пустая или null!");
        else
            isChanged |= ScheduleInfo.TrySetTimeZoneId(timeZoneId, preserveInstant);

        if (dueDateTime.HasValue)
        {
            LocalDateTime localDateTime = dueDateTime.Value;

            isChanged |= ScheduleInfo.TrySetDueDate(localDateTime.Date, preserveInstant);
            isChanged |= ScheduleInfo.TrySetDueTime(localDateTime.TimeOfDay, preserveInstant);
        }

        if (isChanged)
            Changed?.Invoke();
    }

    public void SetDueTime(LocalTime dueTime, bool preserveInstant = false)
    {
        if (ScheduleInfo.TrySetDueTime(dueTime, preserveInstant))
            Changed?.Invoke();
    }

    public void SetDueDate(LocalDate dueDate, bool preserveInstant = false)
    {
        if (ScheduleInfo.TrySetDueDate(dueDate, preserveInstant))
            Changed?.Invoke();
    }

    public void SetTimeZone(string timeZoneId, bool preserveInstant = false)
    {
        if (ScheduleInfo.TrySetTimeZoneId(timeZoneId, preserveInstant))
            Changed?.Invoke();
    }

    public void MarkAsCompleted()
    {
        if (CompletionInfo.TryMarkAsCompleted())
            Changed?.Invoke();
    }

    public void MarkAsUncompleted()
    {
        if (CompletionInfo.TryMarkAsUncompleted())
            Changed?.Invoke();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Changed -= AuditInfo.RecordUpdate;
    }

    internal uint? GetXminForDiagnostics() => 
        _xmin;
}