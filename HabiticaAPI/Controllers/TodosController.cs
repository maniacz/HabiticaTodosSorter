using Microsoft.AspNetCore.Mvc;

namespace HabiticaAPI.Controllers;

[ApiController]
public class TodosController : Controller
{
    private readonly ILogger<TodosController> _logger;

    public TodosController(ILogger<TodosController> logger)
    {
        _logger = logger;
    }

    [HttpGet("todos")]
    public async Task<IActionResult> GetAllTodos()
    {
        
    }
}