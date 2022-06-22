using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.DAL
{
    public interface IUserDAL
    {
        List<User> GetAllUsers();

        User GetUserByEmail(string email);
        Task AddUser(User user);
        Task UpdateUser();
        Task DeleteUser(User user);

        User GetUserByID(int id);
    }
}
