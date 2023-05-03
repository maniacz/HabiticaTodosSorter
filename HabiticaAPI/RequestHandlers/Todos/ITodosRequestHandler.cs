using FluentResults;
using HabiticaAPI.Models;

namespace HabiticaAPI.RequestHandlers.Todos;

public interface ITodosRequestHandler
{
    Task<Result<GetAllTodosResponse>> GetAllTodos();
}