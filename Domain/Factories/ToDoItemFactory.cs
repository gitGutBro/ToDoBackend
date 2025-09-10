using Domain.Entities.ToDoItem;
using Domain.Services;

namespace Domain.Factories;

public class ToDoItemFactory(IScheduleService scheduleService, ICompletionService completionService) : IToDoItemFactory
{
    private readonly IScheduleService _scheduleService = scheduleService ?? throw new ArgumentNullException(nameof(scheduleService));
    private readonly ICompletionService _completionService = completionService ?? throw new ArgumentNullException(nameof(completionService));

    public ToDoItem Create(string title, string? description = null)
    {
        ToDoItem item = new(title, description);
        item.AttachServices(_scheduleService, _completionService);
        return item;
    }
}