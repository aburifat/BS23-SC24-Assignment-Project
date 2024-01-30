using BS23_SC24_Assignment_Backend.Context;
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

        private User? AuthenticateUser(UserLoginRequest user)
        {
            return _context.Users
                .Where(x => x.UserName == user.UserName && x.Password == user.Password)
                .FirstOrDefault();
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.UserRole.Name)
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
                if(user.UserRole == null)
                {
                    user.UserRole = _context.UserRoles
                                .Where(x => x.Id == user.UserRoleId)
                                .FirstOrDefault();
                }
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

        [HttpGet("AuthorizeCheck")]
        [Authorize]
        public IActionResult AuthorizeCheck()
        {
            return Ok();
        }

        [HttpGet("AuthorizeCheck2")]
        [Authorize(Roles = "Administrator, Regular")]
        public IActionResult AuthorizeCheck2()
        {
            return Ok();
        }

        [HttpGet("AdminCheck")]
        [Authorize(Roles = "Administrator")]
        public IActionResult AdminCheck()
        {
            return Ok();
        }

        [HttpGet("RegularCheck")]
        [Authorize(Roles = "Regular")]
        public IActionResult RegularCheck()
        {
            return Ok();
        }
    }
}
