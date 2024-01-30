namespace BS23_SC24_Assignment_Backend.Responses
{
    public class ValidationResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public List<string> Details { get; set; }
    }
}
