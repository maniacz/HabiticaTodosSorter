namespace HabiticaAPI.Models.Responses;

public class GetAllTagsResponse
{
    public bool success { get; set; }
    public Data[] data { get; set; }
    
    public class Data
    {
        public string id { get; set; }
        public string name { get; set; }
        public object challenge { get; set; }
    }
}



