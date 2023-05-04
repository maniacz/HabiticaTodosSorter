using FluentResults;
using HabiticaAPI.Models;
using HabiticaAPI.Models.Responses;

namespace HabiticaAPI.RequestHandlers.Todos;

public interface ITodosRequestHandler
{
    Task<Result<GetAllTodosResponse>> GetAllTodos();
}