using BS23_SC24_Assignment_Backend.Managers.Auth;
using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BS23_SC24_Assignment_Backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(UserLoginRequest request)
        {
            UserLoginResponse response = _authManager.Login(request);
            return StatusCode(response.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(UserRegistrationRequest request)
        {
            BaseResponse response = _authManager.Register(request);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpGet("token-validity-check")]
        public IActionResult TokenValidityCheck()
        {
            BaseResponse response = _authManager.TokenValidityCheck();
            return StatusCode(response.StatusCode, response);
        }
    }
}
