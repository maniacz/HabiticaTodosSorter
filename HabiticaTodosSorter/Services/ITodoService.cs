using HabiticaTodosSorter.Models.Todos;

namespace HabiticaTodosSorter.Services;

public interface ITodoService
{
    ICollection<Todo> GetTaggedForSortingTodos(IEnumerable<Todo> allTodos);
}