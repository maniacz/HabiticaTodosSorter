using HabiticaAPI.Models.Tags;
using HabiticaAPI.Models.Todos;

namespace HabiticaAPI.Services;

public class SorterService : ISorterService
{
    private readonly ILogger<SorterService> _logger;

    public SorterService(ILogger<SorterService> logger)
    {
        _logger = logger;
    }
    public ICollection<Todo> SortTodos(List<Todo> todos, SortedList<int, Tag> tagsOrder)
    {
        todos.Sort(new TodoComparer(tagsOrder, _logger));
        _logger.LogDebug("Sorted todos: {sortedTodos}", todos);

        return todos;
    }
}