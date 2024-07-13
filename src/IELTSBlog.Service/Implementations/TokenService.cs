using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using IELTSBlog.Domain.Constants;
using IELTSBlog.Domain.Entities;
using IELTSBlog.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IELTSBlog.Service.Implementations;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    public TokenService(IConfiguration configuration)
    {
        _config = configuration.GetSection("JWT");
    }

    public string GenerateToken(User user)
    {
        var identityClaims = new Claim[]
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Name", user.Name),
            new Claim("Email", user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };
    
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecurityKey"]!));
        var keyCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        int expiresDay = int.Parse(_config["Lifetime"]!);
        var token = new JwtSecurityToken(
            issuer: _config["Issuer"],
            audience: _config["Audience"],
            claims: identityClaims,
            expires: TimeConstants.Now().AddDays(expiresDay),
            signingCredentials: keyCredentials );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}