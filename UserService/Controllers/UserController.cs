using System;
using System.Collections.Generic;
using System.Linq;
using UserService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UserService.Models;
using Shared.Messaging;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMessagePublisher _messagePublisher;
        public UserController(IUserService userService, IMessagePublisher messagePublisher)
        {
            _userService = userService;
            _messagePublisher = messagePublisher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> AddUser([FromBody] UserRegister user)
        {
            if (_userService.CheckEmail(user.Email) || user.Password=="" || user.Password==null)
            {
                User addUser = new Models.User { Email = user.Email, Name = user.Name, School = user.School };
                await _userService.AddUser(addUser);
                User temp = _userService.GetUserByID(addUser);
                await _messagePublisher.PublishMessageAsync("UserRegistered", new {ID = temp.ID, Email = user.Email, 
                    Password = user.Password });
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
