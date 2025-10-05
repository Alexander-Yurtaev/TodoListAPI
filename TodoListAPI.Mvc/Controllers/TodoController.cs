using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Mvc.Data.TodoList;
using TodoListAPI.Mvc.Repositories;
using TodoListAPI.Mvc.Repositories.Exceptions;

namespace TodoListAPI.Mvc.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromServices]ITodoRepository repository, [FromBody]TodoItem item)
    {
        var newItem = await repository.Create(item);
        return CreatedAtAction(
            nameof(GetById),
            new { id = newItem.Id },
            newItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromServices] ITodoRepository repository, [FromRoute]int id, [FromBody]TodoItem item)
    {
        try
        {
            await repository.Update(id, item);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromServices] ITodoRepository repository, [FromRoute]int id)
    {
        try
        {
            await repository.Delete(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromServices] ITodoRepository repository, [FromQuery]int? page=1, [FromQuery(Name = "size")]int? pageSize=10)
    {
        try
        {
            var items = await repository.GetAll(page!.Value, pageSize!.Value);
            return Ok(items);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromServices] ITodoRepository repository, [FromRoute] int id)
    {
        try
        {
            var item = await repository.Get(id);
            return Ok(item);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("update-db")]
    public async Task<IActionResult> UpdateDb([FromServices]TodoListDbContext db)
    {
        try
        {
            await db.Database.MigrateAsync(CancellationToken.None);
            return Ok("Migrations are completed!");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
