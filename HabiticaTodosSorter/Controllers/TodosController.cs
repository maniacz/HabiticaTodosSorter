using HabiticaTodosSorter.Models.Errors;
using HabiticaTodosSorter.Models.Requests;
using HabiticaTodosSorter.Models.Tags;
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
            return Ok(todosResult.Value);
        }
        else
        {
            return BadRequest(todosResult?.Errors.FirstOrDefault()?.Message);
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

    [HttpGet("todosWithTags")]
    public async Task<IActionResult> GetTodosWithTagsAssigned([FromQuery] GetTodosWithTagsAssignedRequest request)
    {
        request = new GetTodosWithTagsAssignedRequest();
        var tagsAssigned = new List<Tag>
        {
            new Tag { Id = Guid.Parse("958a73fb-d341-4513-83c2-c90c318193b5"), Name = "dom" },
            new Tag { Id = Guid.Parse("2995fb09-6228-4388-9082-7fc657fd7a85"), Name = "pilne wa¿ne" }
        };
        request.Tags = tagsAssigned;

        var todosResult = await _todosRequestHandler.GetTodosWithTagsAssigned(request);
        if (todosResult.IsSuccess)
        {
            return Ok(todosResult.Value);
        }
        else
        {
            return BadRequest(todosResult?.Errors.FirstOrDefault()?.Message);
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