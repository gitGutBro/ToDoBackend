using NodaTime;

namespace ApplicationBackend.Dtos;

public sealed record class UpdateToDoItemDto(string Title, string Description, bool IsCompleted, LocalDate? LocalDate, LocalTime? LocalTime, string? TimeZoneId);