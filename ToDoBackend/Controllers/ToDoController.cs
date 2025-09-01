using Microsoft.AspNetCore.Mvc;
using ToDoBackend.Dtos;
using ToDoBackend.Extensions;
using ToDoBackend.Models;
using ToDoBackend.ResultPattern;
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
        return await this.ExecuteAsync<IEnumerable<ToDoItem>>(
            cancellationToken => _toDoService.GetAllAsync(cancellationToken),
            items => Ok(items),
            cancelToken
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancelToken)
    {
        if (id == Guid.Empty)
            return BadRequest(Error.MissingId.Description);

        return await this.ExecuteAsync<ToDoItem?>(
            cancellationToken => _toDoService.GetByIdAsync(id, cancellationToken),
            item => item is not null ? Ok(item) : NotFound(),
            cancelToken
        );
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateToDoItemDto dto, CancellationToken cancelToken)
    {
        if (dto is null)
            return BadRequest("Элемент не может быть null.");

        return await this.ExecuteAsync<ToDoItem>(
            cancellationToken => _toDoService.CreateAsync(dto, cancellationToken),
            item => CreatedAtAction(nameof(GetById), new { id = item.Id }, item),
            cancelToken
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateToDoItemDto dto, CancellationToken cancelToken)
    {
        if (dto is null)
            return BadRequest("Элемент не может быть null.");

        return await this.ExecuteAsync<ToDoItem>(
            cancellationToken => _toDoService.UpdateAsync(id, dto, cancellationToken),
            () => NoContent(),
            cancelToken
        );
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancelToken)
    {
        if (id == Guid.Empty)
            return BadRequest(Error.MissingId.Description);

        return await this.ExecuteAsync<ToDoItem>(
            cancellationToken => _toDoService.DeleteAsync(id, cancellationToken),
            _ => NoContent(),
            cancelToken
        );
    }
}