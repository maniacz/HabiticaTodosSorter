using HabiticaTodosSorter.Models.Dto;
using HabiticaTodosSorter.Models.Todos;

namespace HabiticaTodosSorter.Models.Responses
{
    public class GetTodoResponse
    {
        public bool Success { get; set; }
        public TaskDto Data { get; set; }
    }
}
