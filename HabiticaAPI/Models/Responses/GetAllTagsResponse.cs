namespace HabiticaAPI.Models.Responses;

public class GetAllTagsResponse
{
    public bool Success { get; set; }
    public TagData[] Data { get; set; }
    
    public class TagData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public object Challenge { get; set; }
    }
}



