using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ProjectVictory.API.Authentication;

public sealed class JwtTokenGenerator
{
    private readonly IConfiguration _configuration;


    public JwtTokenGenerator(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public string Generate(Guid accountId)
    {
        var key =
            _configuration["Jwt:Key"]
            ?? "ProjectVictoryDevelopmentSecretKey123456";


        var securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));


        var credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);


        var claims =
            new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    accountId.ToString())
            };


        var token =
            new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials);


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}