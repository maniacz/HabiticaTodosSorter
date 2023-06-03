using FluentResults;
using HabiticaAPI.Models.Tags;
using HabiticaAPI.Models.Todos;

namespace HabiticaAPI.Services;

public interface ISorterService
{
    ICollection<Todo> GetTodosInFinalOrder(ICollection<Todo> todos, SortedList<int, Tag> tagsOrder);
    Result SortTodos(ICollection<Todo> todosToSort, ICollection<Todo> todosInFinalOrder);
}