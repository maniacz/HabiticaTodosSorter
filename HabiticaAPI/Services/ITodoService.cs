using HabiticaAPI.Models.Todos;

namespace HabiticaAPI.Services;

public interface ITodoService
{
    ICollection<Todo> GetTaggedForSortingTodos(IEnumerable<Todo> allTodos);
}