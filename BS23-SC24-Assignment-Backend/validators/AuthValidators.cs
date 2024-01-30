using BS23_SC24_Assignment_Backend.Context;
using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;
using System.Text.RegularExpressions;

namespace BS23_SC24_Assignment_Backend.validators
{
    public class AuthValidators
    {
        private readonly AppDbContext _context;

        public AuthValidators(AppDbContext context)
        {
            _context = context;
        }

        private bool isStrongPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
        }

        private bool isValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        }

        public ValidationResponse UserRegistrationRequestValidator(UserRegistrationRequest request)
        {
            bool isValid = true;

            string message = "";

            List<string> details = new List<string>();
    
            if (request.UserName == "" || request.Email == "" || request.Password == "" || request.ConfirmPassword == "") // Empty Input Field Case 
            {
                isValid = false;
                details.Add("Input fields can't be empty.");
            }
            else if (request.Password != request.ConfirmPassword) // Password and Confirm Password didn't match
            {
                isValid = false;
                details.Add("Both Password Didn't Match");
            }
            else if (!isStrongPassword(request.Password)) // Weak Password
            {
                isValid = false;
                details.Add("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
            }
            else if (!isValidEmail(request.Email)) // Invalid Email
            {
                isValid = false;
                details.Add("Invalid email format. Please provide a valid email address.");
            }
            else if (_context.Users.Where(x => x.UserName == request.UserName).Any()) // Duplicate UserName
            {
                isValid = false;
                details.Add("Username already taken. Please try another one.");
            }
            else if (_context.Users.Where(x => x.Email == request.Email).Any()) // Duplicate Email Address
            {
                isValid = false;
                details.Add("Email is in use. Try with another Email.");
            }

            if (!isValid) message = "One or more validation errors occurred.";

            ValidationResponse validationResponse = new ValidationResponse
            {
                IsValid = isValid,
                Message = message,
                Details = details
            };
            return validationResponse;
        }

        public ValidationResponse UserLoginRequestValidator(UserLoginRequest request)
        {
            bool isValid = true;

            string message = "";

            List<string> details = new List<string>();

            if (request.UserName == "" || request.Password == "") // Empty Input Field Case 
            {
                isValid = false;
                details.Add("Input fields can't be empty.");
            }
            else if (!isStrongPassword(request.Password)) // Weak Password
            {
                isValid = false;
                details.Add("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
            }

            if (!isValid) message = "One or more validation errors occurred.";

            ValidationResponse validationResponse = new ValidationResponse
            {
                IsValid = isValid,
                Message = message,
                Details = details
            };
            return validationResponse;
        }
    }
}
