using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.DAL
{
    public class UserDAL : IUserDAL
    {
        private readonly UserContext _userContext;

        public UserDAL(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task AddUser(User user)
        {
            _userContext.Users.Add(user);

            await _userContext.SaveChangesAsync();
        }

        public async Task DeleteUser(User user)
        {
            _userContext.Users.Remove(user);

            await _userContext.SaveChangesAsync();
        }

        public List<User> GetAllUsers()
        {
            return _userContext.Users.ToList();
        }

        public User GetUserByEmail(string email)
        {
            return _userContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public User GetUserByID(int id)
        {
            return _userContext.Users.FirstOrDefault(x => x.ID == id);
        }

        public async Task UpdateUser()
        {
            await _userContext.SaveChangesAsync();
        }
    }
}
