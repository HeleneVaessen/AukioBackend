using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.DAL;
using UserService.Models;
namespace UserService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDAL _userDAL;

        public UserService(IUserDAL userDAL)
        {
            _userDAL = userDAL;
        }
        public List<User> GetAllUsers()
        {
            return _userDAL.GetAllUsers();
        }

        public async Task AddUser(User user)
        {
            await _userDAL.AddUser(user);
        }

        public async Task<bool> UpdateUser(User user)
        {
            var temp = _userDAL.GetUserByID(user.ID);
            if ((temp != null) && (!CheckEmail(user.Email)))
            {
                temp.Name = user.Name;
                temp.Email = user.Email;
                temp.School = user.School;
                await _userDAL.UpdateUser();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteUser(User user)
        {
            await _userDAL.DeleteUser(user);

            if (_userDAL.GetUserByID(user.ID) == null)
            {
                return true;
            }
            return false;
        }

        public bool CheckEmail(string email)
        {
            if (_userDAL.GetUserByEmail(email) == null)
            {
                return true;
            }
            return false;
        }

        public User GetUserByID(int userID)
        {
            return _userDAL.GetUserByID(userID);
        }
    }
}
