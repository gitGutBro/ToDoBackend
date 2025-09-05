using Microsoft.EntityFrameworkCore;
using Serilog;
using ToDoBackend.Data;
using ToDoBackend.Models;

namespace ToDoBackend.Repositories;

public class ToDoRepositoryEf(TodoDbContext context) : IToDoRepository
{
    private readonly TodoDbContext _context = context;

    public async Task<Result<ToDoItem>> CreateAsync(ToDoItem item, CancellationToken cancelToken)
    {
        _context.ToDoItems.Add(item);

        try
        {
            await _context.SaveChangesAsync(cancelToken);
            return Result.Ok(item);
        }
        catch (DbUpdateException ex)
        {
            Log.Error(ex, "Ошибка при создании задачи.");
            return Result.Fail(Error.DatabaseError("Ошибка при создании задачи."));
        }
    }

    public Task<List<ToDoItem>> GetAllAsync(CancellationToken cancelToken) =>
        _context.ToDoItems.AsNoTracking().OrderBy(item => item.Id).ToListAsync(cancelToken);

    public async Task<Result<ToDoItem>> GetByIdAsync(Guid id, CancellationToken cancelToken)
    {
        ToDoItem? entity = await _context.ToDoItems.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id, cancelToken);
        return entity is null ? Result.Fail(Error.NotFound(id)) : Result.Ok(entity);
    }

    public async Task<Result> UpdateAsync(ToDoItem item, CancellationToken cancelToken)
    {
        try
        {
            _context.ToDoItems.Update(item);
            int affected = await _context.SaveChangesAsync(cancelToken);
            return affected == 0 ? Result.Fail(ToDoErrors.NotFound(item.Id)) : Result.Ok();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Fail(ToDoErrors.ConcurrencyConflict(item.Id));
        }
        catch (DbUpdateException ex)
        {
            Log.Error(ex, "Ошибка при создании задачи.");
            return Result.Fail(Error.DatabaseError("Ошибка при создании задачи."));
        }
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancelToken)
    {
        ToDoItem? entity = await _context.ToDoItems.FirstOrDefaultAsync(item => item.Id == id, cancelToken);

        if (entity is null) 
            return Result.Fail(ToDoErrors.NotFound(id));

        _context.ToDoItems.Remove(entity);

        try
        {
            await _context.SaveChangesAsync(cancelToken);
            return Result.Ok();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Fail(ToDoErrors.ConcurrencyConflict(id));
        }
    }
}