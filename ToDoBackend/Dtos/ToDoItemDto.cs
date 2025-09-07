using NodaTime;

namespace ToDoBackend.Dtos;

public sealed record class ToDoItemDto(Guid Id, string Title, string? Description, bool IsCompleted, LocalDate? DueDate, LocalTime? DueTime, string? TimeZoneId);