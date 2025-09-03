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

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancelToken)
    {
        return await this.ExecuteAsync
        (
            _ => _toDoService.GetAllAsync(cancelToken),
            items => Ok(items),
            cancelToken
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancelToken)
    {
        return await this.ExecuteAsync
        (
            _ => _toDoService.GetByIdAsync(id, cancelToken),
            item => item is not null ? Ok(item) : NotFound(),
            cancelToken
        );
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateToDoItemDto dto, CancellationToken cancelToken)
    {
        return await this.ExecuteAsync
        (
            _ => _toDoService.CreateAsync(dto, cancelToken),
            item => CreatedAtAction(nameof(GetById), new { id = item.Id }, item),
            cancelToken
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateToDoItemDto dto, CancellationToken cancelToken)
    {
        return await this.ExecuteAsync
        (
            _ => _toDoService.UpdateAsync(id, dto, cancelToken),
            _ => NoContent(),
            cancelToken
        );
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancelToken)
    {
        return await this.ExecuteAsync
        (
            _ => _toDoService.DeleteAsync(id, cancelToken),
            _ => NoContent(),
            cancelToken
        );
    }
}