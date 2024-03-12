namespace HabiticaTodosSorter.Models.Responses
{
    public class ErrorResponse
    {
        public string Error { get; set; }
        public string Message { get; set; }
        public ErrorDetail[] Errors { get; set; }
    }

    public class ErrorDetail
    {
        public string Message { get; set; }
        public  string Param { get; set; }
        public string Value { get; set; }
    }
}
