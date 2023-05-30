using HabiticaAPI.Models.Tags;

namespace HabiticaAPI.Models.Todos;

public class Todo
{
    public string TaskName { get; set; }
    public Guid TaskId { get; set; }
    public int TaskPosition { get; set; }
    public DateTime CreationTime { get; set; }
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public string ListTagNames()
    {
        var tagNames = Tags.Select(t => t.Name);
        return string.Join(',', tagNames);
    }
}