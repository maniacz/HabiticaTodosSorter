namespace HabiticaAPI.Models;

public class GetAllTodosResponse
{
    public bool Success { get; set; }
    public Data[] Data { get; set; }
    public Notifications[] Notifications { get; set; }
    public string AppVersion { get; set; }
}

public class Data
{
    public Challenge Challenge { get; set; }
    public Group Group { get; set; }
    public bool Completed { get; set; }
    public bool CollapseChecklist { get; set; }
    public string Type { get; set; }
    public string Notes { get; set; }
    public string[] Tags { get; set; }
    public double Value { get; set; }
    public double Priority { get; set; }
    public string Attribute { get; set; }
    public bool ByHabitica { get; set; }
    public Checklist[] Checklist { get; set; }
    public object[] Reminders { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    public string Id { get; set; }
    public string Text { get; set; }
    public string UserId { get; set; }
    public string Date { get; set; }
}

public class Challenge
{

}

public class Group
{
    public CompletedBy CompletedBy { get; set; }
    public object[] AssignedUsers { get; set; }
    public Approval Approval { get; set; }
    public string SharedCompletion { get; set; }
}

public class CompletedBy
{

}

public class Approval
{
    public bool Requested { get; set; }
    public bool Approved { get; set; }
    public bool Required { get; set; }
}

public class Checklist
{
    public bool Completed { get; set; }
    public string Text { get; set; }
    public string Id { get; set; }
}

public class Notifications
{
    public string Id { get; set; }
    public Data1 Data { get; set; }
    public bool Seen { get; set; }
    public string Type { get; set; }
}

public class Data1
{
    public string HeaderText { get; set; }
    public string BodyText { get; set; }
    public string Icon { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public string Destination { get; set; }
}

