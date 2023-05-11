using HabiticaAPI.RequestHandlers.Todos;
using Microsoft.AspNetCore.Mvc;

namespace HabiticaAPI.Controllers;

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

    [HttpGet("sort")]
    public async Task<IActionResult> SortTodos()
    {
        var sortResult = await _todosRequestHandler.SortTodos();

        return null;
    }
}