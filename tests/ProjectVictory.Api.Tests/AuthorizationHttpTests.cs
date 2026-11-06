using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-QA-001 parte 2: autenticação (401) e posse de recursos (403) do PV-SEC-001, pela API real.
[Collection(ApiCollection.Name)]
public sealed class AuthorizationHttpTests
{
    private readonly ApiFactory _factory;

    public AuthorizationHttpTests(ApiFactory factory)
    {
        _factory = factory;
    }


    public static TheoryData<string, string> ProtectedEndpoints => new()
    {
        { "GET", "/api/character" },
        { "POST", "/api/character" },
        { "POST", "/api/game/enter" },
        { "POST", "/api/game/heartbeat" }, // PV-RUNTIME-010
        { "POST", "/api/game/logout" }, // PV-HUNT-012
        { "GET", "/api/item/11111111-1111-1111-1111-111111111111" },
        { "POST", "/api/hunt/execute" },
        { "POST", "/api/hunt/attack" },
        { "POST", "/api/hunt/leave" }, // PV-HUNT-011
        { "POST", "/api/hunt-session/start" },
        { "POST", "/api/hunt-session/stop" },
        { "GET", "/api/inventory/22222222-2222-2222-2222-222222222222" },
        { "POST", "/api/runtime/create" },
        { "POST", "/api/hunt/supplies" }, // PV-SUPPLY-010
        { "POST", "/api/runtime/activity" },
        { "POST", "/api/runtime/location" },
        { "POST", "/api/gameplay/action" },
        { "POST", "/api/location/actions" },
        { "GET", "/api/playable/status" },
    };


    [Theory]
    [MemberData(nameof(ProtectedEndpoints))]
    public async Task ProtectedEndpoint_WithoutToken_Returns401(string method, string url)
    {
        using var client = _factory.CreateClient();

        using var request = new HttpRequestMessage(new HttpMethod(method), url);

        if (method == "POST")
        {
            request.Content = JsonContent.Create(new { });
        }

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    [Fact]
    public async Task Account_IsPublic()
    {
        using var client = _factory.CreateClient();

        var token = await _factory.RegisterAndLoginAsync(client, $"public-{Guid.NewGuid():N}@test.com");

        Assert.False(string.IsNullOrWhiteSpace(token));
    }


    [Fact]
    public async Task ValidToken_IsAccepted()
    {
        using var client = _factory.CreateClient();

        var token = await _factory.RegisterAndLoginAsync(client, $"valid-{Guid.NewGuid():N}@test.com");

        var response = await ApiFactory.WithToken(client, token).GetAsync("/api/playable/status");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    [Theory]
    [InlineData("OutroIssuer", ApiFactory.Audience)]
    [InlineData(ApiFactory.Issuer, "OutraAudience")]
    public async Task TokenWithWrongIssuerOrAudience_Returns401(string issuer, string audience)
    {
        using var client = _factory.CreateClient();

        var token = CreateToken(ApiFactory.JwtKey, issuer, audience);

        var response = await ApiFactory.WithToken(client, token).GetAsync("/api/playable/status");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    [Fact]
    public async Task TokenSignedWithOtherKey_Returns401()
    {
        using var client = _factory.CreateClient();

        var token = CreateToken(Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + "-outra-chave-de-teste", ApiFactory.Issuer, ApiFactory.Audience);

        var response = await ApiFactory.WithToken(client, token).GetAsync("/api/playable/status");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    [Fact]
    public async Task OtherAccount_CannotUseCharacter_Returns403()
    {
        using var ownerClient = _factory.CreateClient();
        using var otherClient = _factory.CreateClient();

        var ownerToken = await _factory.RegisterAndLoginAsync(ownerClient, $"owner-{Guid.NewGuid():N}@test.com");
        var otherToken = await _factory.RegisterAndLoginAsync(otherClient, $"other-{Guid.NewGuid():N}@test.com");

        ApiFactory.WithToken(ownerClient, ownerToken);
        ApiFactory.WithToken(otherClient, otherToken);

        var created =
            await ownerClient.PostAsJsonAsync(
                "/api/character",
                new { name = $"H{Guid.NewGuid():N}"[..16], characterClass = "Knight" });

        created.EnsureSuccessStatusCode();

        var characterId = await created.Content.ReadFromJsonAsync<Guid>();

        (await ownerClient.PostAsync("/api/game/enter", null)).EnsureSuccessStatusCode();

        // Dono: 200.
        Assert.Equal(HttpStatusCode.OK, (await ownerClient.GetAsync($"/api/inventory/{characterId}")).StatusCode);

        // Outra conta: 403, sem detalhes do recurso.
        var inventory = await otherClient.GetAsync($"/api/inventory/{characterId}");
        Assert.Equal(HttpStatusCode.Forbidden, inventory.StatusCode);
        Assert.DoesNotContain(characterId.ToString(), await inventory.Content.ReadAsStringAsync());

        Assert.Equal(
            HttpStatusCode.Forbidden,
            (await otherClient.PostAsJsonAsync("/api/hunt-session/start", new { characterId })).StatusCode);

        Assert.Equal(
            HttpStatusCode.Forbidden,
            (await otherClient.PostAsJsonAsync("/api/hunt/execute", new { characterId, runtimeId = Guid.NewGuid() })).StatusCode);

        Assert.Equal(
            HttpStatusCode.Forbidden,
            (await otherClient.PostAsJsonAsync("/api/runtime/create", new { runtimeId = Guid.NewGuid(), characterId })).StatusCode);

        Assert.Equal(
            HttpStatusCode.Forbidden,
            (await otherClient.PostAsJsonAsync("/api/gameplay/action", new { actionId = Guid.NewGuid(), characterId })).StatusCode);
    }


    private static string CreateToken(string key, string issuer, string audience)
    {
        var credentials =
            new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: [new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())],
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
