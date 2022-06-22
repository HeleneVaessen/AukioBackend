using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Messaging;
using System.Threading.Tasks;
using UserService.Models;
using UserService.Services;
using System;

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
            if (_userService.CheckEmail(user.Email) || user.Password != "" || user.Password != null)
            {
                User addUser = new Models.User { Email = user.Email, Name = user.Name, School = user.School };
                await _userService.AddUser(addUser);
                User temp = _userService.GetUserByID(addUser.ID);
                await _messagePublisher.PublishMessageAsync("UserRegistered", new
                {
                    ID = temp.ID,
                    Email = user.Email,
                    Password = user.Password
                });
                return Ok();
            }
            return BadRequest("Email already taken");
        }

        [HttpPost("getUserData")]
        public IActionResult GetUserData([FromBody] int ID)
        {
            System.Console.WriteLine("getUserData");
            User userData = _userService.GetUserByID(ID);
            return Ok(userData);
        }

        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdate user)
        {
            if (_userService.CheckEmail(user.Email) || user.Password != "" || user.Password != null)
            {
                User addUser = new Models.User {ID= user.ID, Email = user.Email, Name = user.Name, School = user.School };
                await _userService.UpdateUser(addUser);
                
                await _messagePublisher.PublishMessageAsync("UserUpdated", new
                {
                    ID = user.ID,
                    Email = user.Email,
                    Password = user.Password,
                    NewPassword =user.NewPassword
                });
                return Ok();
            }
            else return BadRequest("User could not be updated");
        }

        [HttpDelete("deleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] User user)
        {
            if (await _userService.DeleteUser(user)) return Ok();
            return BadRequest("User could not be deleted");
        }
    }
}
