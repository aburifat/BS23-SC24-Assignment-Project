using BS23_SC24_Assignment_Backend.Enums;

namespace BS23_SC24_Assignment_Backend.Managers.Security
{
    public interface IAuthenticatedUser
    {
        public long Id { get; }
        public string? UserName { get; }
        public UserRole UserRole { get; }
    }
}
