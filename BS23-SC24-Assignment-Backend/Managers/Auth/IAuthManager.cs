using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;

namespace BS23_SC24_Assignment_Backend.Managers.Auth
{
    public interface IAuthManager
    {
        UserLoginResponse Login(UserLoginRequest request);
        BaseResponse Register(UserRegistrationRequest request);
        BaseResponse TokenValidityCheck();
    }
}
