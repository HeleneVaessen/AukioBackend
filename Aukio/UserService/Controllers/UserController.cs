using System;
using System.Collections.Generic;
using System.Linq;
using UserService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (_userService.CheckEmail(user.Email))
            {
                await _userService.AddUser(user);
                return Ok();
            }
            return BadRequest("Email already taken");
        }
       
        [HttpGet("getUsers")]
        public IActionResult GetUsers()
        {
            List<User> users =_userService.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody]User user)
        {
            if (await _userService.UpdateUser(user)) return Ok();
            else return BadRequest("User could not be updated");
        }

        [HttpDelete("deleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody]User user)
        {
            if (await _userService.DeleteUser(user)) return Ok();
            return BadRequest("User could not be deleted");
        }
}
}
