using AuthenticationService.Models;

namespace AuthenticationService.Services
{
    public interface IAuthService
    {
        void AddUser(User user);
        string Login(User user);
        void ChangeUser(UserUpdated user);
        User GetUser(User user);
        void DeleteUser(User user);

    }
}
