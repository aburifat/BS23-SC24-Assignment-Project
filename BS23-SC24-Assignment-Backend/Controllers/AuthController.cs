using BS23_SC24_Assignment_Backend.Context;
using BS23_SC24_Assignment_Backend.Enums;
using BS23_SC24_Assignment_Backend.Models;
using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BS23_SC24_Assignment_Backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthController(IConfiguration configuration, AppDbContext appDbContext)
        {
            _configuration = configuration;
            _context = appDbContext;

        }

        private User? AuthenticateUser(UserLoginRequest request)
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.UserName == request.UserName);

            if(existingUser != null && BCrypt.Net.BCrypt.Verify(request.Password, existingUser.Password))
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
            var user = AuthenticateUser(request);

            if (user != null)
            {
                var userLoginResponse = new UserLoginResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    UserRole = user.UserRole,
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
            bool isValid = true;
            string message = "";
            if (request.Password != request.ConfirmPassword) {
                isValid = false;
                message = "Both Password Didn't Match";
            }
            else if(_context.Users.Where(x=>x.UserName == request.UserName).Any())
            {
                isValid = false;
                message = "Username already taken. Please try another one.";
            }else if(_context.Users.Where(x => x.Email == request.Email).Any())
            {
                isValid = false;
                message = "Email is in use. Try with another Email.";
            }

            if (!isValid)
            {
                return BadRequest(message);
            }

            // Password is hashed using BCrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

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
