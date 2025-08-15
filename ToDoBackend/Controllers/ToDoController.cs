using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Add([FromBody] ToDoItem item)
    {
        if (item == null)
            return BadRequest("Элемент не может быть null.");

        await _toDoService.AddAsync(item);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ToDoItem item)
    {
        if (id == Guid.Empty || item == null)
            return BadRequest("Идентификатор не может быть пустым и должен совпадать с идентификатором элемента.");

        item.UpdateId(id);
        await _toDoService.UpdateAsync(item);
        return NoContent();
    }
}