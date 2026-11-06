using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ProjectVictory.API.Authentication;

public sealed class JwtTokenGenerator
{
    private readonly JwtSettings _settings;


    public JwtTokenGenerator(
        IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }


    public string Generate(Guid accountId)
    {
        var securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.Key));


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
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials);


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}
