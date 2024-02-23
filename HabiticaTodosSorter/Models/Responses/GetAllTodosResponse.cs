using HabiticaTodosSorter.Models.Dto;

namespace HabiticaTodosSorter.Models.Responses;

public class GetAllTodosResponse
{
    public bool Success { get; set; }
    public TaskDto[] Data { get; set; }
    public Notifications[] Notifications { get; set; }
    public string AppVersion { get; set; }
}