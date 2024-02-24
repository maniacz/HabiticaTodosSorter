using FluentResults;
using HabiticaTodosSorter.Models;
using HabiticaTodosSorter.Models.Requests;
using HabiticaTodosSorter.Models.Responses;
using HabiticaTodosSorter.Models.Todos;

namespace HabiticaTodosSorter.RequestHandlers.Todos;

public interface ITodosRequestHandler
{
    Task<Result<IList<Todo>>> GetAllTodos();
    Task<Result<Todo>> GetTodo(GetTodoRequest request);
    Task<Result> SortTodos(ICollection<Todo> todosToSort);
    Task<Result<List<Todo>>> GetTodosWithTagsAssigned(GetTodosWithTagsAssignedRequest request);
}