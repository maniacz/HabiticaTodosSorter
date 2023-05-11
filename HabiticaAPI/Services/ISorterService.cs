using HabiticaAPI.Models.Todos;

namespace HabiticaAPI.Services;

public interface ISorterService
{
    ICollection<Todo> SortTodos(ICollection<Todo> todos);
}