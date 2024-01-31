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

        public ValidationResponse UserRegistrationRequestValidator(UserRegistrationRequest request)
        {
            bool isValid = true;

            string message = "";
    
            if (request.UserName == "" || request.Email == "" || request.Password == "" || request.ConfirmPassword == "") // Empty Input Field Case 
            {
                isValid = false;
                message = "Input fields can't be empty.";
            }
            else if (request.Password != request.ConfirmPassword) // Password and Confirm Password didn't match
            {
                isValid = false;
                message = "Both Password Didn't Match";
            }
            else if (!IsStrongPassword(request.Password)) // Weak Password
            {
                isValid = false;
                message = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
            }
            else if (!IsValidEmail(request.Email)) // Invalid Email
            {
                isValid = false;
                message = "Invalid email format. Please provide a valid email address.";
            }
            else if (_context.Users.Where(x => x.UserName == request.UserName).Any()) // Duplicate UserName
            {
                isValid = false;
                message = "Username already taken. Please try another one.";
            }
            else if (_context.Users.Where(x => x.Email == request.Email).Any()) // Duplicate Email Address
            {
                isValid = false;
                message = "Email is in use. Try with another Email.";
            }

            ValidationResponse validationResponse = new()
            {
                IsValid = isValid,
                Message = message
            };
            return validationResponse;
        }

        public ValidationResponse UserLoginRequestValidator(UserLoginRequest request)
        {
            bool isValid = true;

            string message = "";

            if (request.UserName == "" || request.Password == "") // Empty Input Field Case 
            {
                isValid = false;
                message = "Input fields can't be empty.";
            }
            else if (!IsStrongPassword(request.Password)) // Weak Password
            {
                isValid = false;
                message = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
            }

            ValidationResponse validationResponse = new()
            {
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
