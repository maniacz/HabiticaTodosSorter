namespace HabiticaTodosSorter.Models.Responses;

public class MoveTodoToNewPositionResponse
{
    public bool Success { get; set; }
    public string[] Data { get; set; }
    public object[] Notifications { get; set; }
}