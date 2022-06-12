using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.DAL;
using UserService.Models;

namespace UserService.Test
{
    class MockDAL : IUserDAL
    {

        public List<User> testUsers;

        public MockDAL()
        {
            testUsers = new List<User>();
        }
        public Task AddUser(User user)
        {
            testUsers.Add(user);
            return Task.CompletedTask;
        }

        public Task DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            return testUsers;
        }

        public User GetUserByEmail(string email)
        {
            return testUsers.FirstOrDefault(user => user.Email == email);
        }

        public User GetUserByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser()
        {
            throw new NotImplementedException();
        }
    }
}
