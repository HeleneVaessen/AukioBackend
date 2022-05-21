using AuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Services
{
    public interface IJwtAuthenticationManager
    {
        string WriteToken(int ID, Roles role);
        int ReadToken(string token);
        string GetRole(string token);

    }
}
