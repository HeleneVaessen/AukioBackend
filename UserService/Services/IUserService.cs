using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Services
{
    public interface IUserService
    {
        bool CheckEmail(string email);
        List<User> GetAllUsers();
        Task AddUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(User user);
        User GetUserByID(int userID);
    }
}
