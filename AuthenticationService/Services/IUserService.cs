using AuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Services
{
    public interface IUserService
    {
        void AddUser(int id, string email, string password);
        string Authenticate(string email, string password);
        void ChangeUser(int id, string email);
        User GetUser(int id);
        void DeleteUser(int id);

    }
}
