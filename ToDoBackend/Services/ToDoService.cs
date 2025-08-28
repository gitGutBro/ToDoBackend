using System.Threading;
using ToDoBackend.Dtos;
using ToDoBackend.Mappers;
using ToDoBackend.Models;
using ToDoBackend.Repositories;
using ToDoBackend.ResultPattern;

namespace ToDoBackend.Services;

public class ToDoService(IToDoRepository toDoRepository, CreateToDoMapper createMapper, UpdateToDoMapper updateMapper) : IToDoService
{
    private readonly IToDoRepository _toDoRepository = toDoRepository ?? throw new ArgumentNullException(nameof(toDoRepository));
    private readonly CreateToDoMapper _createMapper = createMapper ?? throw new ArgumentNullException(nameof(createMapper));
    private readonly UpdateToDoMapper _updateMapper = updateMapper ?? throw new ArgumentNullException(nameof(updateMapper));

    public async Task<Result<IEnumerable<ToDoItem>>> GetAllAsync(CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            return await Task.FromCanceled<Result<IEnumerable<ToDoItem>>>(cancelToken);

        return await _toDoRepository.GetAllAsync(cancelToken);
    }

    public async Task<Result<ToDoItem?>> GetByIdAsync(Guid id, CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            return await Task.FromCanceled<Result<ToDoItem?>>(cancelToken);

        return await _toDoRepository.GetByIdAsync(id, cancelToken);
    }

    public async Task<Result<ToDoItem>> CreateAsync(CreateToDoItemDto dto, CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            cancelToken.ThrowIfCancellationRequested();

        ToDoItem item = _createMapper.MapToModel(dto);

        return await _toDoRepository.CreateAsync(item, cancelToken);
    }

    public async Task<Result<ToDoItem>> UpdateAsync(Guid id, UpdateToDoItemDto dto, CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            cancelToken.ThrowIfCancellationRequested();

        Result<ToDoItem?> getResult = await _toDoRepository.GetByIdAsync(id, cancelToken);

        Task<Result<ToDoItem>> matchResult = getResult.Match(
            existingToDo =>
            {
                if (existingToDo is null)
                {
                    return Task.FromResult(Result<ToDoItem>.Failure(ToDoErrors.NotFound));
                }

                _updateMapper.UpdateModel(existingToDo, dto);
                return _toDoRepository.UpdateAsync(existingToDo, cancelToken);
            },
            error =>
            {
                return Task.FromResult(Result<ToDoItem>.Failure(error));
            }
        );

        return await matchResult;
    }

    public async Task<Result<ToDoItem>> DeleteAsync(Guid id, CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            return await Task.FromCanceled<Result<ToDoItem>>(cancelToken);

        return await _toDoRepository.DeleteAsync(id, cancelToken);
    }
}