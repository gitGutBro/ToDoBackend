using NodaTime;

namespace ToDoBackend.Dtos;

public record class UpdateToDoItemDto(string Title, string Description, bool IsCompleted, LocalDate? LocalDate, LocalTime? LocalTime, string? TimeZoneId, bool PreserveScheduleAtInstance);