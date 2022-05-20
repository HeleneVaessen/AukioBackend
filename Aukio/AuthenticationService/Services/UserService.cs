using AuthenticationService.DAL;
using AuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Services
{
    public class UserService :IUserService
    {
        private readonly IUserDAL _userDAL;

        private readonly IHashingService _cryptographyService;

        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public UserService(IUserDAL userDAL, IHashingService cryptographyService, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _userDAL = userDAL;
            _cryptographyService = cryptographyService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        public void AddUser(int id, string email, string password)
        {
            var salt = _cryptographyService.GenerateSalt();

            var hashedPassword = _cryptographyService.HashPassword(password, salt);

            var stringSalt = Convert.ToBase64String(salt);

            _userDAL.AddUser(new User { ID = id, Email = email, Password = hashedPassword, Role = Roles.Guest, Salt = stringSalt });
        }

        public string Authenticate(string email, string password)
        {
            var user = _userDAL.GetUserByEmail(email);

            if (user != null && user.Password == _cryptographyService.HashInput(password, user.Salt))
            {
                var result = _userDAL.GetUserByEmail(email);
                return _jwtAuthenticationManager.WriteToken(result.ID, result.Role);
            }

            return null;
        }

        public void ChangeUser(int id, string email)
        {
            var user = _userDAL.GetUserById(id);

            if (user != null)
            {
                user.Email = email;
                _userDAL.Save();
            }
        }

        public User GetUser(int id)
        {
            return _userDAL.GetUserById(id);
        }

        public void DeleteUser(int id)
        {
            var user = _userDAL.GetUserById(id);

            if (user != null)
            {
                _userDAL.DeleteUser(user);
            }
        }

    }
}
