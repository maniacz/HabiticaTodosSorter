using HabiticaAPI.Models.Todos;

namespace HabiticaAPI.Services;

public class SorterService : ISorterService
{
    public ICollection<Todo> SortTodos(ICollection<Todo> todos)
    {
        foreach (var todo in todos)
        {
            // todo.Tags.Contains()
        }

        return null;
    }
}