using BS23_SC24_Assignment_Backend.Context;
using BS23_SC24_Assignment_Backend.Models;
using BS23_SC24_Assignment_Backend.Requests;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BS23_SC24_Assignment_Backend.Managers.Security
{
    public class AuthUtils(IConfiguration config, AppDbContext context)
    {
        private AppDbContext _context = context;
        private IConfiguration _config = config;

        public User? AuthenticatedUser(UserLoginRequest request)
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.UserName == request.UserName);

            if (existingUser != null && BCrypt.Net.BCrypt.Verify(request.Password, existingUser.Password))
            {
                return existingUser;
            }
            return null;
        }

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Role, user.UserRole.ToString())
            };

            var token = new JwtSecurityToken(
                _config["JwtSettings:Issuer"],
                _config["JwtSettings:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
                );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }
    }
}
