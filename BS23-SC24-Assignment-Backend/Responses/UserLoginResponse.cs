using BS23_SC24_Assignment_Backend.Enums;

namespace BS23_SC24_Assignment_Backend.Responses
{
    public class UserLoginResponse
    {
        public bool IsValid {  get; set; }
        public string Message { get; set; }
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserRole UserRole { get; set; }
        public string UserRoleName { get; set; }
        public string AccessToken { get; set; }
    }
}
