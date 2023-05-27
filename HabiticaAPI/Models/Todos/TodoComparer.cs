using HabiticaAPI.Models.Tags;

namespace HabiticaAPI.Models.Todos;

public class TodoComparer : IComparer<Todo>
{
    private readonly SortedList<int, Tag> _tagsOrder;

    public TodoComparer(SortedList<int, Tag> tagsOrder)
    {
        _tagsOrder = tagsOrder;
    }
    
    public int Compare(Todo? x, Todo? y)
    {
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
        xTagPriority = _tagsOrder.Where(t => t.Value.Name == xTagForSorting?.Name).Select(t => t.Key).FirstOrDefault();

        yTagForSorting = y.Tags.FirstOrDefault(tag => _tagsOrder.Values.Any(orderedTag => orderedTag.Name == tag.Name));
        yTagPriority = _tagsOrder.Where(t => t.Value.Name == yTagForSorting?.Name).Select(t => t.Key).FirstOrDefault();

        return xTagPriority.CompareTo(yTagPriority);
    }
}