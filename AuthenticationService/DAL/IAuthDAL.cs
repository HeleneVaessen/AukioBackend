using AuthenticationService.Models;
using System.Threading.Tasks;

namespace AuthenticationService.DAL
{
    public interface IAuthDAL
    {
        Task AddUser(User user);

        Task UpdateUser();

        Task DeleteUser(User user);

        User GetUserByEmail(string email);

        User GetUserByUserId(int userId);

    }
}
