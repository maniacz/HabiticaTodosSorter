using HabiticaTodosSorter.Constants;
using HabiticaTodosSorter.Models.Todos;

namespace HabiticaTodosSorter.Services;

public class TodoService : ITodoService
{
    public ICollection<Todo> GetTaggedForSortingTodos(IEnumerable<Todo> allTodos)
    {
        return allTodos.Where(todo => todo.Tags.Any(tag => AppConstants.SortingTagNames.Contains(tag.Name))).ToList();
    }
}