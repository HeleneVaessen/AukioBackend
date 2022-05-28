using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Messaging;
using AuthenticationService.Models;
using AuthenticationService.Services;

namespace AuthenticationService.Messaging
{
    public class UserRegisteredMessage : IMessageHandler<User>
    {
        private IAuthService _userService;

        public UserRegisteredMessage(IAuthService userService)
        {
            _userService = userService;
        }
        public Task HandleMessageAsync(string messageType, User message)
        {
            _userService.AddUser(message.ID, message.Email, message.Password);

            return Task.CompletedTask;
        }
    }
}
