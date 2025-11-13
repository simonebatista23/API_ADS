using ApiUsers.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiUsers.Services
{
    public static class TokenServices
    {
        public static object GenerateToken(User user)
        {

            var key = Encoding.UTF8.GetBytes(Key.secret);

            var claims = new List<Claim>
            {
                 new Claim("idUser", user.Id.ToString()),
                 new Claim("unique_name", user.Name ?? ""),
                 new Claim("email", user.Email ?? ""),
                 new Claim("IsAdmin", (user.IdDeptNavigation?.Name == "rh" && user.IdProfileNavigation?.Desc == "superadmin").ToString()),


                new Claim("idDept", user.IdDeptNavigation?.Id.ToString() ?? "0"),
                new Claim("department", user.IdDeptNavigation?.Name ?? ""),
                new Claim("idProfile", user.IdProfileNavigation?.Id.ToString() ?? "0"),
                new Claim("profile", user.IdProfileNavigation?.Desc ?? "")
            };

            if (user.IdDeptNavigation?.Name?.ToLower() == "rh" &&
            user.IdProfileNavigation?.Desc?.ToLower() == "superadmin")
            {
                claims.Add(new Claim("IsAdmin", "true"));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(3), 
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new
            {
                token = tokenString,
                expiration = tokenDescriptor.Expires
            };
        }
    }
}
