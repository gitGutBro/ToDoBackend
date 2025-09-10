using ApplicationBackend.Repositories;
using Domain.Entities.ToDoItem;
using Domain.Services;
using InfrastructureBackend.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.ResultPattern;

namespace InfrastructureBackend.Repositories;

public class ToDoRepository(ToDoItemDbContext context, IScheduleService scheduleService, ICompletionService completionService) : IToDoRepository
{
    private readonly ToDoItemDbContext _context = context;
    private readonly IScheduleService _scheduleService = scheduleService;
    private readonly ICompletionService _completionService = completionService;

    public async Task<Result<ToDoItem>> CreateAsync(ToDoItem item, CancellationToken cancellationToken)
    {
        item.AttachServices(_scheduleService, _completionService);

        _context.ToDoItems.Add(item);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return Result<ToDoItem>.Success(item);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            Log.Warning("Операция создания задачи была отменена.");
            return Result<ToDoItem>.Failure(Error.OperatinCanceled);
        }
        catch (DbUpdateException ex)
        {
            Log.Error(ex, "Ошибка при создании задачи.");
            return Result<ToDoItem>.Failure(Error.DatabaseError);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Неожиданная ошибка при создании задачи.");
            return Result<ToDoItem>.Failure(Error.UnknownError);
        }
    }

    public async Task<Result<IEnumerable<ToDoItem>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            List<ToDoItem> items = await _context.ToDoItems
                .AsNoTracking()
                .OrderBy(item => item.Id)
                .ToListAsync(cancellationToken);

            foreach (var it in items)
                it.AttachServices(_scheduleService, _completionService);

            return Result<IEnumerable<ToDoItem>>.Success(items);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            Log.Warning("Операция получения всех задач была отменена.");
            return Result<IEnumerable<ToDoItem>>.Failure(Error.OperatinCanceled);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ошибка при получении всех задач.");
            return Result<IEnumerable<ToDoItem>>.Failure(Error.UnknownError);
        }
    }

    public async Task<Result<ToDoItem?>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            ToDoItem? gotItem = await _context.ToDoItems
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

            if (gotItem is null)
                return Result<ToDoItem?>.Failure(Error.NotFoundWithId(id));

            gotItem.AttachServices(_scheduleService, _completionService);

            return Result<ToDoItem?>.Success(gotItem);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            Log.Warning("Операция получения задачи по Id была отменена.");
            return Result<ToDoItem?>.Failure(Error.OperatinCanceled);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ошибка при получении задачи по Id {Id}.", id);
            return Result<ToDoItem?>.Failure(Error.UnknownError);
        }
    }

    public async Task<Result<ToDoItem>> UpdateAsync(ToDoItem item, CancellationToken cancellationToken)
    {
        try
        {
            item.AttachServices(_scheduleService, _completionService);

            _context.ToDoItems.Update(item);
            int affected = await _context.SaveChangesAsync(cancellationToken);

            return affected == 0
                ? Result<ToDoItem>.Failure(Error.NotFoundWithId(item.Id))
                : Result<ToDoItem>.Success(item);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            Log.Warning("Операция обновления задачи была отменена.");
            return Result<ToDoItem>.Failure(Error.OperatinCanceled);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Log.Error(ex, "Ошибка конкуретности базы данных при обновлении задачи с Id {Id}.", item.Id);
            return Result<ToDoItem>.Failure(Error.DatabaseConcurrencyErrorWithId(item.Id));
        }
        catch (DbUpdateException ex)
        {
            Log.Error(ex, "Ошибка при обновлении задачи.");
            return Result<ToDoItem>.Failure(Error.DatabaseError);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Неожиданная ошибка при обновлении задачи.");
            return Result<ToDoItem>.Failure(Error.UnknownError);
        }
    }

    public async Task<Result<ToDoItem>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        ToDoItem? itemToDelete = await _context.ToDoItems.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (itemToDelete is null) 
            return Result<ToDoItem>.Failure(Error.NotFoundWithId(id));

        itemToDelete.AttachServices(_scheduleService, _completionService);

        _context.ToDoItems.Remove(itemToDelete);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return Result<ToDoItem>.Success(itemToDelete);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            Log.Warning($"Операция удаления задачи была отменена.");
            return Result<ToDoItem>.Failure(Error.OperatinCanceled);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Log.Error($"Ошибка конкуретности базы данных при удалении задачи с Id: {itemToDelete.Id}. Ошибка: {ex}");
            return Result<ToDoItem>.Failure(Error.DatabaseConcurrencyErrorWithId(itemToDelete.Id));
        }
        catch (Exception ex)
        {
            Log.Error($"Ошибка при удалении задачи: {ex}");
            return Result<ToDoItem>.Failure(Error.UnknownError);
        }
    }
}