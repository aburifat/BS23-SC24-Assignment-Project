using BS23_SC24_Assignment_Backend.Context;
using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;
using System.Text.RegularExpressions;

namespace BS23_SC24_Assignment_Backend.validators
{
    public partial class AuthValidators(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        private static bool IsStrongPassword(string password)
        {
            return MyPasswordRegex().IsMatch(password);
        }

        private static bool IsValidEmail(string email)
        {
            return MyEmailRegex().IsMatch(email);
        }

        public BaseResponse UserRegistrationRequestValidator(UserRegistrationRequest request)
        {
            bool isValid = false;

            string message = "";
    
            if (string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                message = "Input fields can't be empty.";
            }
            else if (request.Password != request.ConfirmPassword)
            {
                message = "Both Password Didn't Match";
            }
            else if (!IsStrongPassword(request.Password))
            {
                message = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
            }
            else if (!IsValidEmail(request.Email))
            {
                message = "Invalid email format. Please provide a valid email address.";
            }
            else if (_context.Users.Where(x => x.UserName == request.UserName).Any())
            {
                message = "Username already taken. Please try another one.";
            }
            else if (_context.Users.Where(x => x.Email == request.Email).Any())
            {
                message = "Email is in use. Try with another Email.";
            }
            else
            {
                isValid = true;
            }

            BaseResponse validationResponse = new()
            {
                StatusCode = (isValid) ? 200 : 400,
                IsValid = isValid,
                Message = message
            };
            return validationResponse;
        }

        public UserLoginResponse UserLoginRequestValidator(UserLoginRequest request)
        {
            bool isValid = false;

            string message = "";

            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password)) // Empty Input Field Case 
            {
                message = "Input fields can't be empty.";
            }
            else if (!IsStrongPassword(request.Password)) // Weak Password
            {
                message = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
            }
            else
            {
                isValid = true;
            }

            UserLoginResponse validationResponse = new()
            {
                StatusCode = (isValid) ? 200 : 400,
                IsValid = isValid,
                Message = message
            };
            return validationResponse;
        }

        [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")]
        private static partial Regex MyPasswordRegex();
        [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        private static partial Regex MyEmailRegex();
    }
}
