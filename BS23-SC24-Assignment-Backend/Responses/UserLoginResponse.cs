using BS23_SC24_Assignment_Backend.Enums;

namespace BS23_SC24_Assignment_Backend.Responses
{
    public class UserLoginResponse
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserRole UserRole { get; set; }
        public string UserRoleName { get; set; }
        public string AccessToken { get; set; }
    }
}
