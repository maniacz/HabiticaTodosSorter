using HabiticaTodosSorter.Models.Errors;
using HabiticaTodosSorter.Models.Requests;
using HabiticaTodosSorter.RequestHandlers.Todos;
using Microsoft.AspNetCore.Mvc;

namespace HabiticaTodosSorter.Controllers;

[ApiController]
public class TodosController : Controller
{
    private readonly ILogger<TodosController> _logger;
    private readonly ITodosRequestHandler _todosRequestHandler;

    public TodosController(ITodosRequestHandler todosRequestHandler, ILogger<TodosController> logger)
    {
        _logger = logger;
        _todosRequestHandler = todosRequestHandler;
    }

    [HttpGet("todos")]
    public async Task<IActionResult> GetAllTodos()
    {
        var todosResult = await _todosRequestHandler.GetAllTodos();

        if (todosResult.IsSuccess)
        {
            return Ok(todosResult);
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpGet("todo")]
    public async Task<IActionResult> GetTodo([FromQuery] GetTodoRequest request)
    {
        var todoResult = await _todosRequestHandler.GetTodo(request);

        if (todoResult.IsSuccess)
        {
            return Ok(todoResult.Value);
        }
        else
        {
            return BadRequest(todoResult?.Errors.FirstOrDefault()?.Message);
        }
    }

    [HttpGet("sort")]
    public async Task<IActionResult> SortTodos()
    {
        var todosResult = await _todosRequestHandler.GetAllTodos();
        if (todosResult.IsFailed)
        {
            return BadRequest();
        }

        var sortResult = await _todosRequestHandler.SortTodos(todosResult.Value.ToList());
        if (sortResult.IsSuccess)
        {
            return Ok("Todos were successfully ordered.");
        }
        else if (sortResult.HasError<NoDataError>())
        {
            return NotFound(sortResult);
        }

        return BadRequest(sortResult);
    }
}