using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs.Auth;
using UserService.Application.Services;

namespace UserService.Presentation.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly JwtService _jwt;

        public AuthController(AuthService authService, JwtService jwt)
        {
            _authService = authService;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = await _authService.RegisterAsync(request.Username, request.Password);

            if (user == null)
                return BadRequest("Username already exist!");

            var token = _jwt.GenerateToken(user);

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _authService.LoginAsync(request.Username, request.Password);

            if (user == null)
                return Unauthorized();

            var token = _jwt.GenerateToken(user);

            return Ok(new { token });
        }
    }
}
