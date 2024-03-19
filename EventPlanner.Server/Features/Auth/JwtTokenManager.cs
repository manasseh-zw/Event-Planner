using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventPlanner.Server.Data.Models;
using Microsoft.IdentityModel.Tokens;

namespace EventPlanner.Server.Features.Auth;

public interface IJwtTokenManager
{
    string GenerateToken(User user);
}
public class JwtTokenManager : IJwtTokenManager
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;


    public JwtTokenManager(IConfiguration configuration)
    {
        _secretKey = configuration.GetSection("JwtConfig:Secret").Value;
        _issuer = configuration.GetSection("JwtConfig:Issuer").Value;
        _audience = configuration.GetSection("JwtConfig:Audience").Value;
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
