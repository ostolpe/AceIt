using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AceIt.Models;
using Anthropic.Models.Beta.Environments;
using Microsoft.IdentityModel.Tokens;

namespace AceIt.Services;

public class JwtService(IConfiguration config)
{
    public string GenerateToken(User user)
    {
        var secret = Encoding.UTF8.GetBytes(config["Jwt:Secret"]!)
            ?? throw new Exception("Could not find Jwt Secret");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
        };

        var key = new SymmetricSecurityKey(secret);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "Backend",
            audience: "Frontend",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
