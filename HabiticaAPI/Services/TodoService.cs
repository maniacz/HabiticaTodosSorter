using HabiticaAPI.Constants;
using HabiticaAPI.Models.Todos;

namespace HabiticaAPI.Services;

public class TodoService : ITodoService
{
    public ICollection<Todo> GetTaggedForSortingTodos(IEnumerable<Todo> allTodos)
    {
        return allTodos.Where(todo => todo.Tags.Any(tag => AppConstants.SortingTagNames.Contains(tag.Name))).ToList();
    }
}