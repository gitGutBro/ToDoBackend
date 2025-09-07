namespace ApplicationBackend.Dtos;

public sealed record class CreateToDoItemDto(string Title, string? Description);