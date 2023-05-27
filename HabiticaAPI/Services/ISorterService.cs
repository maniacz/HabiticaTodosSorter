using HabiticaAPI.Models.Tags;
using HabiticaAPI.Models.Todos;

namespace HabiticaAPI.Services;

public interface ISorterService
{
    ICollection<Todo> SortTodos(List<Todo> todos, SortedList<int, Tag> tagsOrder);
}