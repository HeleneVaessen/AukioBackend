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
        private IAuthService _authService;

        public UserRegisteredMessage(IAuthService authService)
        {
            _authService = authService;
        }
        public Task HandleMessageAsync(string messageType, User user)
        {
            _authService.AddUser(user);

            return Task.CompletedTask;
        }
    }
}
