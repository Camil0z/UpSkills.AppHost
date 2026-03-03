using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UpSkills.Api.Interfaces;

namespace UpSkills.Api.Services;

public class AuthService : IAuthService
{
    private readonly string _apiKey = "Api/Jwt/TokenKey/auth/user_Sesions/";

    public string GenerateToken(string userId)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_apiKey));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.Name, userId) }
            ),
            Expires = DateTime.UtcNow.AddHours(1), // Duración de la sesión
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_apiKey);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
