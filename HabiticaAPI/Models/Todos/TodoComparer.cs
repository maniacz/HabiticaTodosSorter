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

        var xTagForSorting = x.Tags.FirstOrDefault(tag => _tagsOrder.Values.Any(orderedTag => orderedTag.Name == tag.Name));
        var xTagPriority = xTagForSorting is null ? lowestPriority : _tagsOrder.Where(t => t.Value.Name == xTagForSorting?.Name).Select(t => t.Key).FirstOrDefault();

        var yTagForSorting = y.Tags.FirstOrDefault(tag => _tagsOrder.Values.Any(orderedTag => orderedTag.Name == tag.Name));
        var yTagPriority = yTagForSorting is null ? lowestPriority : _tagsOrder.Where(t => t.Value.Name == yTagForSorting?.Name).Select(t => t.Key).FirstOrDefault();

        if (xTagPriority == yTagPriority)
        {
            // Assign higher priority to newest todos
            if (x.CreationTime > y.CreationTime)
                xTagPriority = -1;
            else
                yTagPriority = -1;
        }

        _logger.LogDebug("\rx: {xTodoName}\n \t{xTagNames}\n y: {yTodoName}\n \t{yTagNames}", x.TaskName, x.ListTagNames(), y.TaskName, y.ListTagNames());
        _logger.LogDebug(xTagPriority.CompareTo(yTagPriority).ToString() + Environment.NewLine);
        
        return xTagPriority.CompareTo(yTagPriority);
    }
}