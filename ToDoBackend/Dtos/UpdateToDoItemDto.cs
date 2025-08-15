using System.Text.Json.Serialization;

namespace ToDoBackend.Dtos;

public class UpdateToDoItemDto
{
    [JsonIgnore] public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
}