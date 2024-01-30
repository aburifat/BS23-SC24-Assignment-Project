using BS23_SC24_Assignment_Backend.Models;

namespace BS23_SC24_Assignment_Backend.Responses
{
    public class UserLoginResponse
    {
        public long Id { get; set; }
        public string? UserName { get; set; }
        public UserRole UserRole { get; set; }
        public string? AccessToken { get; set; }
    }
}
