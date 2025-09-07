using Microsoft.AspNetCore.Mvc;
using ToDoBackend.Dtos;
using ToDoBackend.Extensions;
using ToDoBackend.Services;

namespace ToDoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoController(IToDoService toDoService) : ControllerBase
{
    private readonly IToDoService _toDoService = toDoService ?? throw new ArgumentNullException(nameof(toDoService));

    [HttpGet("all")]
    public Task<IActionResult> GetAll(CancellationToken cancellationToken) =>
        this.ExecuteAsync(_toDoService.GetAllAsync, Ok, cancellationToken);

    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken) =>
        this.ExecuteAsync(funcCancellationToken => _toDoService.GetByIdAsync(id, funcCancellationToken),
                          item => item is not null ? Ok(item) : NotFound(),
                          cancellationToken);

    [HttpPost]
    public Task<IActionResult> Add([FromBody] CreateToDoItemDto dto, CancellationToken cancellationToken) =>
        this.ExecuteAsync(funcCancellationToken => _toDoService.CreateAsync(dto, funcCancellationToken),
                          item => CreatedAtAction(nameof(GetById), new { id = item.Id }, item),
                          cancellationToken);

    [HttpPut("{id:guid}")]
    public Task<IActionResult> Update(Guid id, [FromBody] UpdateToDoItemDto dto, CancellationToken cancellationToken) =>
        this.ExecuteAsyncNoResult(funcCancellationToken => _toDoService.UpdateAsync(id, dto, funcCancellationToken), NoContent, cancellationToken);

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken) =>
        this.ExecuteAsyncNoResult(funcCancellationToken => _toDoService.DeleteAsync(id, funcCancellationToken), NoContent, cancellationToken);
}