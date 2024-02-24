using HabiticaTodosSorter.Models.Tags;

namespace HabiticaTodosSorter.Models.Requests
{
    public class GetTodosWithTagsAssignedRequest
    {
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
