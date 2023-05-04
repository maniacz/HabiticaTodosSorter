using FluentResults;
using HabiticaAPI.Clients;
using HabiticaAPI.Models;
using HabiticaAPI.Models.Responses;
using Microsoft.AspNetCore.Routing.Matching;

namespace HabiticaAPI.RequestHandlers.Todos;

public class TodosRequestHandler : ITodosRequestHandler
{
    private readonly IHabiticaClient _habiticaClient;
    private readonly ILogger<TodosRequestHandler> _logger;

    public TodosRequestHandler(IHabiticaClient habiticaClient, ILogger<TodosRequestHandler> logger)
    {
        _habiticaClient = habiticaClient;
        _logger = logger;
    }
    
    public async Task<Result<GetAllTodosResponse>> GetAllTodos()
    {
        var result = await _habiticaClient.GetAllTodos();

        if (result.IsFailed)
        {
            return result.ToResult();
        }

        return Result.Ok(result.Value);
    }
}