using Microsoft.AspNetCore.Mvc;
using System;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Models;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public AuthenticationController(IUserService userService, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _userService = userService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost("authenticate")] 
        public IActionResult Authenticate([FromBody] User user)
        {
            var token = _userService.Authenticate(user.Email, user.Password);

            if (token == null) return Unauthorized();

            return Ok(token);
        }

        [HttpPost("readToken")]
        public IActionResult ReadToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            var id = _jwtAuthenticationManager.ReadToken(token);

            return Ok(id);
        }

        [HttpGet("getRole")]
        public IActionResult GetRole()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            var role = _jwtAuthenticationManager.GetRole(token);

            return Ok(role);
        }

    }
}
