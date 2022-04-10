using AuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.DAL
{
    public interface IUserDAL
    {
        Task AddUser(User user);

        User GetUserByEmail(string email);

        User GetUserById(int userId);

        Task Save();

        Task DeleteUser(User user);
    }
}
