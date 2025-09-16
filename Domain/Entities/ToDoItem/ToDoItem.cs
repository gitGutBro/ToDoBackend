using Domain.Helpers;
using NodaTime;
using Shared.Extensions;
using Shared.ResultPattern;
using System.Text.Json.Serialization;

namespace Domain.Entities.ToDoItem;

public class ToDoItem : IEntity, IDisposable
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

    public Result<bool> SetScheduledInfo(LocalDateTime? dueDateTime, bool preserveInstant, string? timeZoneId = null) => 
        ScheduleHelper.TrySetScheduledInfo(ScheduleInfo, dueDateTime, preserveInstant, timeZoneId)
            .HandleEntityChange(() => Changed?.Invoke(), "SetScheduleInfo", Id);

    public Result<bool> SetDueTime(LocalTime dueTime, bool preserveInstant = false) =>
        ScheduleHelper.TrySetDueTime(ScheduleInfo, dueTime, preserveInstant)
            .HandleEntityChange(() => Changed?.Invoke(), "SetDueTime", Id);

    public Result<bool> SetDueDate(LocalDate dueDate, bool preserveInstant = false) => 
        ScheduleHelper.TrySetDueDate(ScheduleInfo, dueDate, preserveInstant)
            .HandleEntityChange(() => Changed?.Invoke(), "SetDueDate", Id);

    public Result<bool> SetTimeZone(string timeZoneId, bool preserveInstant = false) => 
        ScheduleHelper.TrySetTimeZoneId(ScheduleInfo, timeZoneId, preserveInstant)
            .HandleEntityChange(() => Changed?.Invoke(), "SetTimeZone", Id);

    public Result<bool> MarkAsCompleted() =>
        CompletionHelper.TryMarkAsCompleted(CompletionInfo)
            .HandleEntityChange(() => Changed?.Invoke(), "MarkAsCompleted", Id);

    public Result<bool> MarkAsUncompleted() =>
        CompletionHelper.TryMarkAsUncompleted(CompletionInfo)
            .HandleEntityChange(() => Changed?.Invoke(), "MarkAsUncompleted", Id);

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Changed -= AuditInfo.RecordUpdate;
    }

    internal uint? GetXminForDiagnostics() => 
        _xmin;
}