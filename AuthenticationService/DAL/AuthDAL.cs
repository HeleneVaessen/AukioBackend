using AuthenticationService.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.DAL
{
    public class AuthDAL : IAuthDAL
    {
        private readonly UserContext _userContext;
        public AuthDAL(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task AddUser(User user)
        {
            _userContext.Add(user);
            await _userContext.SaveChangesAsync();
        }


        public async Task UpdateUser()
        {
            await _userContext.SaveChangesAsync();
        }

        public async Task DeleteUser(User user)
        {
            _userContext.Remove(user);
            await _userContext.SaveChangesAsync();
        }

        public User GetUserByEmail(string email)
        {
            return _userContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public User GetUserByUserId(int userId)
        {
            return _userContext.Users.FirstOrDefault(x => x.ID == userId);
        }
    }
}
