﻿namespace BS23_SC24_Assignment_Backend.Responses
{
    public class GetTaskResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public long UserId { get; set; }
    }
}
