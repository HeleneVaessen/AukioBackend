using AuthenticationService.Models;

namespace AuthenticationService.Services
{
    public interface IJwtAuthenticationManager
    {
        string TurnIntoJWTToken(int ID, Roles role);
        int TranslateToId(string token);
        string GetUserRole(string token);

    }
}
