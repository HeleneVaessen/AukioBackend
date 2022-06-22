using AuthenticationService.DAL;
using AuthenticationService.Models;
using System;

namespace AuthenticationService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthDAL _authDAL;

        private readonly IHashingService _iHashingService;

        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public AuthService(IAuthDAL authDAL, IHashingService hashingservice, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _authDAL = authDAL;
            _iHashingService = hashingservice;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        public void AddUser(User user)
        {
            var salt = _iHashingService.GenerateSalt();

            var passwordHashed = _iHashingService.HashPassword(user.Password, salt);

            var saltString = Convert.ToBase64String(salt);

            _authDAL.AddUser(new User { ID = user.ID, Email = user.Email, Password = passwordHashed, Role = Roles.Guest, Salt = saltString });
        }

        public string Login(User user)
        {
            var temp = _authDAL.GetUserByEmail(user.Email);

            if (temp != null && temp.Password == _iHashingService.HashInput(user.Password, temp.Salt))
            {
                return _jwtAuthenticationManager.TurnIntoJWTToken(temp.ID, temp.Role);
            }

            return null;
        }

        public void ChangeUser(UserUpdated user)
        {
            var salt = _iHashingService.GenerateSalt();

            var passwordHashed = _iHashingService.HashPassword(user.NewPassword, salt);

            var saltString = Convert.ToBase64String(salt);

            var temp = _authDAL.GetUserByUserId(user.ID);
            
            if (temp != null)
            {
                temp.Email = user.Email;
                temp.Password = passwordHashed;
                temp.Salt = saltString;
                _authDAL.UpdateUser();
            }
        }

        public User GetUser(User user)
        {
            return _authDAL.GetUserByUserId(user.ID);
        }

        public void DeleteUser(User user)
        {
            var temp = _authDAL.GetUserByUserId(user.ID);

            if (temp != null)
            {
                _authDAL.DeleteUser(temp);
            }
        }

    }
}
