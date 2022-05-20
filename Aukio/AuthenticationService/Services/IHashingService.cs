using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Services
{
    public interface IHashingService
    {
        string HashPassword(string password, byte[] salt);
        byte[] GenerateSalt();
        string HashInput(string password, string salt);

    }
}
