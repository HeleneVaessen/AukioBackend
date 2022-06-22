using AuthenticationService.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AuthenticationService.Services
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly string JWTKey;

        public JwtAuthenticationManager()
        {
            this.JWTKey = "JWTKeyForAukioCreatedIn2022";
        }

        public string TurnIntoJWTToken(int ID, Roles role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JWTKey);
            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, ID.ToString()),
                    new Claim(ClaimTypes.Role, role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }

        public int TranslateToId(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var claim = securityToken.Claims.First().Value;

            return int.Parse(claim);
        }

        public string GetUserRole(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var role = securityToken.Claims.Where(e => e.Type == "role").FirstOrDefault().Value;

            return role.ToString();
        }
    }
}
