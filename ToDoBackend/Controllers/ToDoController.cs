using Microsoft.AspNetCore.Mvc;
using ToDoBackend.Dtos;
using ToDoBackend.Models;
using ToDoBackend.Services;

namespace ToDoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoController : ControllerBase
{
    private readonly IToDoService _toDoService;

    public ToDoController(IToDoService toDoService) => 
        _toDoService = toDoService ?? throw new ArgumentNullException(nameof(toDoService));

    [HttpGet]
    public async Task<IActionResult> GetAll() => 
        Ok(await _toDoService.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("Идентификатор не может быть пустым.");

        ToDoItem? item = await _toDoService.GetByIdAsync(id);
        return item != null ? Ok(item) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateToDoItemDto dto)
    {
        if (dto == null)
            return BadRequest("Элемент не может быть null.");

        ToDoItem item = await _toDoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, dto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateToDoItemDto dto)
    {
        if (id == Guid.Empty || dto == null)
            return BadRequest("Идентификатор не может быть пустым и должен совпадать с идентификатором элемента.");

        await _toDoService.UpdateAsync(id, dto);
        return NoContent();
    }
}