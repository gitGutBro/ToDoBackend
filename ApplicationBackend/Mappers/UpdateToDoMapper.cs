using ApplicationBackend.Dtos;
using Domain.Entities.ToDoItem;
using Shared.ResultPattern;

namespace ApplicationBackend.Mappers;

public class UpdateToDoMapper : IUpdateMapper<ToDoItem, UpdateToDoItemDto>
{
    public Result<bool> UpdateEntity(ToDoItem item, UpdateToDoItemDto dto)
    {
        item.UpdateTitle(dto.Title);
        item.UpdateDescription(dto.Description);

        Result<bool> setResult = item.SetScheduledInfo(dto.LocalDate + dto.LocalTime, dto.PreserveInstant, dto.TimeZoneId);
        
        if (setResult.IsFailure)
            return Result<bool>.Failure(setResult.Error);

        Result<bool> markResult;

        if (dto.IsCompleted)
            markResult = item.MarkAsCompleted();
        else
            markResult = item.MarkAsUncompleted();

        if (markResult.IsFailure)
            return Result<bool>.Failure(markResult.Error);

        return Result<bool>.Success(true);
    }
}