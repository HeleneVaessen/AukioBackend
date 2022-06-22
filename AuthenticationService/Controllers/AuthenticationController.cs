using AuthenticationService.Models;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public AuthenticationController(IAuthService authService, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _authService = authService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            var jwt = _authService.Login(user);

            if (jwt == null) return Unauthorized();

            return Ok(jwt);
        }


        [HttpGet("getRole")]
        public IActionResult GetRole()
        {
            var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            var role = _jwtAuthenticationManager.GetUserRole(jwt);

            return Ok(role);
        }

        [HttpPost("translatetoID")]
        public IActionResult translateToID()
        {
            var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            var id = _jwtAuthenticationManager.TranslateToId(jwt);

            return Ok(id);
        }


    }
}
