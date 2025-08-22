using System.Text.Json.Serialization;

namespace ToDoBackend.Dtos;

public record class UpdateToDoItemDto(string Title, bool IsCompleted);