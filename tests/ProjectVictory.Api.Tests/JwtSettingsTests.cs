using ProjectVictory.API.Authentication;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-QA-001 parte 2 (ressalva R1 do PV-PLATFORM-002): validação que derruba o startup.
public sealed class JwtSettingsTests
{
    private const string ValidKey = "0123456789abcdef0123456789abcdef";

    private static JwtSettings Settings(string key, string issuer = "iss", string audience = "aud") =>
        new() { Key = key, Issuer = issuer, Audience = audience };


    [Fact]
    public void Valid_DoesNotThrow()
    {
        Settings(ValidKey).Validate(isDevelopment: false);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void MissingKey_Throws(string key)
    {
        var ex = Assert.Throws<InvalidOperationException>(
            () => Settings(key).Validate(isDevelopment: false));

        Assert.Contains("Jwt:Key não configurada", ex.Message);
    }

    [Fact]
    public void ShortKey_Throws_WithLength()
    {
        var ex = Assert.Throws<InvalidOperationException>(
            () => Settings(new string('k', JwtSettings.MinimumKeyLength - 1)).Validate(isDevelopment: false));

        Assert.Contains("Jwt:Key fraca", ex.Message);
        Assert.Contains($"tem {JwtSettings.MinimumKeyLength - 1} caracteres", ex.Message);
    }

    [Fact]
    public void KeyWithMinimumLength_IsAccepted()
    {
        Settings(new string('k', JwtSettings.MinimumKeyLength)).Validate(isDevelopment: false);
    }

    [Fact]
    public void MissingIssuerAndAudience_Throws()
    {
        var ex = Assert.Throws<InvalidOperationException>(
            () => Settings(ValidKey, issuer: "", audience: "").Validate(isDevelopment: false));

        Assert.Contains("Jwt:Issuer não configurado", ex.Message);
        Assert.Contains("Jwt:Audience não configurado", ex.Message);
    }

    [Fact]
    public void Development_Message_PointsToUserSecrets()
    {
        var ex = Assert.Throws<InvalidOperationException>(
            () => Settings("").Validate(isDevelopment: true));

        Assert.Contains("dotnet user-secrets set", ex.Message);
    }

    [Fact]
    public void NonDevelopment_Message_PointsToEnvironmentVariable()
    {
        var ex = Assert.Throws<InvalidOperationException>(
            () => Settings("").Validate(isDevelopment: false));

        Assert.Contains("Jwt__Key", ex.Message);
        Assert.DoesNotContain("user-secrets", ex.Message);
    }
}
