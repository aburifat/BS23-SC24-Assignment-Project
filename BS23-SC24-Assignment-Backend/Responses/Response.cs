namespace BS23_SC24_Assignment_Backend.Responses
{
    public class BaseResponse
    {
        public int StatusCode { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
}
