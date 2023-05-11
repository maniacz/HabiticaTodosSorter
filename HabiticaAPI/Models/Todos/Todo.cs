using HabiticaAPI.Models.Tags;

namespace HabiticaAPI.Models.Todos;

public class Todo
{
    public string TaskName { get; set; }
    public Guid TaskId { get; set; }
    public int TaskPosition { get; set; }
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}