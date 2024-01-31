using BS23_SC24_Assignment_Backend.Enums;
using System.Security.Claims;

namespace BS23_SC24_Assignment_Backend.Managers.Security
{
    public class AuthenticatedUser : IAuthenticatedUser
    {
        public long Id { get; private set; }
        public string UserName { get; private set; }
        public UserRole UserRole { get; private set; }

        public AuthenticatedUser(IHttpContextAccessor httpContextAccessor)
        {
            Populate(httpContextAccessor);
        }

        private void Populate(IHttpContextAccessor _httpContextAccessor)
        {
            if (_httpContextAccessor.HttpContext == null) return;

            var currentuser = _httpContextAccessor.HttpContext.User;

            foreach(var claim in currentuser.Claims)
            {
                switch (claim.Type)
                {
                    case ClaimTypes.NameIdentifier:
                        if(long.TryParse(claim.Value, out long id)){
                            Id = id;
                        }
                        break;
                    case ClaimTypes.Name:
                        UserName = claim.Value;
                        break;
                    case ClaimTypes.Role:
                        if(Enum.TryParse(claim.Value, out UserRole userRole)){
                            UserRole = userRole;
                        }
                        break;
                }

            }
        }
    }
}
