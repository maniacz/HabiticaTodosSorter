using FluentResults;
using HabiticaTodosSorter.Models;
using HabiticaTodosSorter.Models.Responses;
using HabiticaTodosSorter.Models.Todos;

namespace HabiticaTodosSorter.RequestHandlers.Todos;

public interface ITodosRequestHandler
{
    Task<Result<IList<Todo>>> GetAllTodos();
    Task<Result> SortTodos(ICollection<Todo> todosToSort);
}