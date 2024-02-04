using BS23_SC24_Assignment_Backend.Context;
using BS23_SC24_Assignment_Backend.Enums;
using BS23_SC24_Assignment_Backend.Managers.Security;
using BS23_SC24_Assignment_Backend.Models;
using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;
using BS23_SC24_Assignment_Backend.validators;

namespace BS23_SC24_Assignment_Backend.Managers.Auth
{
    public class AuthManager : IAuthManager
    {
        private readonly AppDbContext _context;
        private readonly AuthValidators _authValidators;
        private readonly AuthUtils _authUtils;

        public AuthManager(AppDbContext context, AuthValidators authValidators, AuthUtils authUtils)
        {
            _context = context;
            _authValidators = authValidators;
            _authUtils = authUtils;
        }

        public UserLoginResponse Login(UserLoginRequest request)
        {
            UserLoginResponse validationResponse = _authValidators.UserLoginRequestValidator(request);

            if (!validationResponse.IsValid)
            {
                return validationResponse;
            }

            var user = _authUtils.AuthenticatedUser(request);

            if (user != null)
            {
                var userLoginResponse = new UserLoginResponse
                {
                    StatusCode = 200,
                    IsValid = true,
                    Message = "Login Successful",
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    UserRole = user.UserRole,
                    UserRoleName = user.UserRole.ToString(),
                    AccessToken = _authUtils.GenerateToken(user)
                };
                return userLoginResponse;
            }
            validationResponse.StatusCode = 401;
            validationResponse.IsValid = false;
            validationResponse.Message = "Invalid Credentials";
            return validationResponse;
        }

        public BaseResponse Register(UserRegistrationRequest request)
        {
            BaseResponse validationResponse = _authValidators.UserRegistrationRequestValidator(request); // validation for the register data

            if (!validationResponse.IsValid)
            {
                return validationResponse;
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            User newUser = new()
            {
                UserName = request.UserName,
                Password = hashedPassword,
                Email = request.Email,
                UserRole = UserRole.Regular
            };

            _context.Users.Add(newUser);

            int isSuccessful = _context.SaveChanges();

            if (isSuccessful > 0)
            {
                validationResponse.StatusCode = 200;
                validationResponse.IsValid = true;
                validationResponse.Message = "Registration successful";
                return validationResponse;
            }
            else
            {
                validationResponse = new BaseResponse()
                {
                    StatusCode = 500,
                    IsValid = false,
                    Message = "Internal Server Error"
                };
                return validationResponse;
            }
        }

        public BaseResponse TokenValidityCheck()
        {
            return new BaseResponse
            {
                StatusCode = 200,
                IsValid = true,
                Message = "Token is valid"
            };
        }
    }
}
