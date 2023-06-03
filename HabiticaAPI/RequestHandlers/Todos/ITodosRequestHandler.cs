using FluentResults;
using HabiticaAPI.Models;
using HabiticaAPI.Models.Responses;
using HabiticaAPI.Models.Todos;

namespace HabiticaAPI.RequestHandlers.Todos;

public interface ITodosRequestHandler
{
    Task<Result<IList<Todo>>> GetAllTodos();
    Task<Result<ICollection<Todo>>> SortTodos(ICollection<Todo> todosToSort);
}