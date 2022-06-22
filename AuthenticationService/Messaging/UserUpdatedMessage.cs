using AuthenticationService.Models;
using AuthenticationService.Services;
using Shared.Messaging;
using System.Threading.Tasks;

namespace AuthenticationService.Messaging
{
    public class UserUpdatedMessage : IMessageHandler<UserUpdated>
    {
        private IAuthService _authService;

        public UserUpdatedMessage(IAuthService authService)
        {
            _authService = authService;
        }
        public Task HandleMessageAsync(string messageType, UserUpdated user)
        {
            _authService.ChangeUser(user);

            return Task.CompletedTask;
        }
    }
}
