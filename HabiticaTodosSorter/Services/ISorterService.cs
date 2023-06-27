using FluentResults;
using HabiticaTodosSorter.Models.Tags;
using HabiticaTodosSorter.Models.Todos;

namespace HabiticaTodosSorter.Services;

public interface ISorterService
{
    ICollection<Todo> GetTodosInFinalOrder(ICollection<Todo> todos, SortedList<int, Tag> tagsOrder);
    Task<Result> SortTodos(ICollection<Todo> todosToSort, ICollection<Todo> todosInFinalOrder);
}