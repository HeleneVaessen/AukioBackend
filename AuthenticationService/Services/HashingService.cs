using System;
using System.Security.Cryptography;

namespace AuthenticationService.Services
{
    public class HashingService : IHashingService
    {
        public byte[] GenerateSalt()
        {
            var salt = new byte[32];

            new RNGCryptoServiceProvider().GetBytes(salt);

            return salt;
        }
        public string HashPassword(string password, byte[] salt)
        {
            var key = new Rfc2898DeriveBytes(password, salt);

            byte[] hash = key.GetBytes(32);

            return Convert.ToBase64String(hash);
        }


        public string HashInput(string password, string salt)
        {
            var byteSalt = Convert.FromBase64String(salt);

            var key = new Rfc2898DeriveBytes(password, byteSalt);

            byte[] hash = key.GetBytes(32);

            return Convert.ToBase64String(hash);
        }
    }
}
