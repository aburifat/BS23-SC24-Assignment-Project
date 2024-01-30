using BS23_SC24_Assignment_Backend.Context;
using BS23_SC24_Assignment_Backend.Enums;
using BS23_SC24_Assignment_Backend.Models;
using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;
using BS23_SC24_Assignment_Backend.validators;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace BS23_SC24_Assignment_Backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly AuthValidators _authValidators;

        public AuthController(IConfiguration configuration, AppDbContext appDbContext, AuthValidators authValidators)
        {
            _configuration = configuration;
            _context = appDbContext;
            _authValidators = authValidators;
        }

        private User AuthenticateUser(UserLoginRequest request)
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.UserName == request.UserName);

            if (existingUser != null && BCrypt.Net.BCrypt.Verify(request.Password, existingUser.Password))
            {
                return existingUser;
            }
            return null;
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
                );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(UserLoginRequest request)
        {
            ValidationResponse validationResponse = _authValidators.UserLoginRequestValidator(request); // validation for the login input

            if (!validationResponse.IsValid)
            {
                return BadRequest(validationResponse);
            }

            var user = AuthenticateUser(request);

            if (user != null)
            {
                var userLoginResponse = new UserLoginResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    UserRole = user.UserRole,
                    UserRoleName = user.UserRole.ToString(),
                    AccessToken = GenerateToken(user)
                };
                return Ok(userLoginResponse);
            }
            return Unauthorized("Invalid credentials");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(UserRegistrationRequest request)
        {
            ValidationResponse validationResponse = _authValidators.UserRegistrationRequestValidator(request); // validation for the register data

            if (!validationResponse.IsValid)
            {
                return BadRequest(validationResponse);
            }

            // Password is hashed using BCrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Creating Ragular User
            User newUser = new User
            {
                UserName = request.UserName,
                Password = hashedPassword,
                Email = request.Email,
                UserRole = UserRole.Regular
            };
            
            _context.Users.Add(newUser);
            int isSuccessful = _context.SaveChanges();
            if(isSuccessful > 0)
            {
                return Ok("Registration successful");
            }
            else
            {
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }
    }
}
