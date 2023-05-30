using HabiticaAPI.Models.Tags;

namespace HabiticaAPI.Models.Todos;

public class TodoComparer : IComparer<Todo>
{
    private readonly SortedList<int, Tag> _tagsOrder;
    private readonly ILogger _logger;

    public TodoComparer(SortedList<int, Tag> tagsOrder, ILogger logger)
    {
        _tagsOrder = tagsOrder;
        _logger = logger;
    }
    
    public int Compare(Todo? x, Todo? y)
    {
        const int lowestPriority = 100;
        
        if (x == null)
        {
            if (y == null)
                return 0;
            return -1;
        }

        if (y == null)
            return 1;
        
        Tag? xTagForSorting, yTagForSorting;
        int xTagPriority, yTagPriority;

        xTagForSorting = x.Tags.FirstOrDefault(tag => _tagsOrder.Values.Any(orderedTag => orderedTag.Name == tag.Name));
        xTagPriority = xTagForSorting is null ? lowestPriority : _tagsOrder.Where(t => t.Value.Name == xTagForSorting?.Name).Select(t => t.Key).FirstOrDefault();

        yTagForSorting = y.Tags.FirstOrDefault(tag => _tagsOrder.Values.Any(orderedTag => orderedTag.Name == tag.Name));
        yTagPriority = yTagForSorting is null ? lowestPriority : _tagsOrder.Where(t => t.Value.Name == yTagForSorting?.Name).Select(t => t.Key).FirstOrDefault();

        _logger.LogDebug("\rx: {xTodoName}\n \t{xTagNames}\n y: {yTodoName}\n \t{yTagNames}", x.TaskName, x.ListTagNames(), y.TaskName, y.ListTagNames());
        _logger.LogDebug(xTagPriority.CompareTo(yTagPriority).ToString() + Environment.NewLine);
        
        return xTagPriority.CompareTo(yTagPriority);
    }
}