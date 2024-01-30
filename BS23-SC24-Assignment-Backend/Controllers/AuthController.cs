using BS23_SC24_Assignment_Backend.Context;
using BS23_SC24_Assignment_Backend.Models;
using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BS23_SC24_Assignment_Backend.Controllers
{
    [Route("api")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthController(IConfiguration configuration, AppDbContext appDbContext)
        {
            _configuration = configuration;
            _context = appDbContext;

        }

        private User? AuthenticatedUser(UserLoginRequest user)
        {
            return _context.Users
                .Where(x => x.UserName == user.UserName && x.Password == user.Password)
                .FirstOrDefault();
        }

        private string GenerateToken(string userName)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginRequest request)
        {
            var _userLoginResponse = new UserLoginResponse
            {
                Id = 0,
                UserName = null,
                AccessToken = null
            };
            var _user = AuthenticatedUser(request);
            if (_user != null)
            {
                _userLoginResponse.Id = _user.Id;
                _userLoginResponse.UserName = _user.UserName;
                _userLoginResponse.AccessToken = GenerateToken(_user.UserName);
            }

            return Ok(_userLoginResponse);
        }
    }
}
