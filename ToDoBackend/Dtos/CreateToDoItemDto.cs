namespace ToDoBackend.Dtos;

public record class CreateToDoItemDto(string? Title, bool IsCompleted);