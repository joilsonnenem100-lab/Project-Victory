using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-QA-003: lacunas de teste deixadas pelo PV-QA-001 parte 2 —
// 403 por HTTP em runtime, hunt-session e encounter com token de outra conta;
// token expirado; token válido sem a claim da conta.
[Collection("Api")]
public sealed class OwnershipAndTokenHttpTests
{
    private readonly ApiFactory _factory;

    public OwnershipAndTokenHttpTests(ApiFactory factory)
    {
        _factory = factory;
    }

    private async Task<HttpClient> NewClient(string prefix)
    {
        var client = _factory.CreateClient();
        var token = await _factory.RegisterAndLoginAsync(client, $"{prefix}-{Guid.NewGuid():N}@test.com");
        return ApiFactory.WithToken(client, token);
    }

    private static async Task<Guid> CreateCharacter(HttpClient client)
    {
        var created = await client.PostAsJsonAsync(
            "/api/character",
            new { name = TestNames.Unique("Q"), characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        return await created.Content.ReadFromJsonAsync<Guid>();
    }

    private static string CreateToken(IEnumerable<Claim> claims, DateTime notBefore, DateTime expires)
    {
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApiFactory.JwtKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: ApiFactory.Issuer,
            audience: ApiFactory.Audience,
            claims: claims,
            notBefore: notBefore,
            expires: expires,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Fact]
    public async Task OtherAccount_CannotChangeRuntime_Returns403()
    {
        using var owner = await NewClient("rt-owner");
        using var other = await NewClient("rt-other");
        var characterId = await CreateCharacter(owner);
        var runtimeId = Guid.NewGuid();

        (await owner.PostAsJsonAsync("/api/runtime/create", new { runtimeId, characterId })).EnsureSuccessStatusCode();

        var activity = await other.PostAsJsonAsync("/api/runtime/activity", new { runtimeId, activity = "Exploring" });
        var location = await other.PostAsJsonAsync(
            "/api/runtime/location",
            new { runtimeId, worldId = Guid.NewGuid(), regionId = Guid.NewGuid(), locationId = Guid.NewGuid() });

        Assert.Equal(HttpStatusCode.Forbidden, activity.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, location.StatusCode);

        // Controle: o dono continua conseguindo mudar a atividade.
        Assert.Equal(
            HttpStatusCode.OK,
            (await owner.PostAsJsonAsync("/api/runtime/activity", new { runtimeId, activity = "Exploring" })).StatusCode);
    }

    [Fact]
    public async Task OtherAccount_CannotStopHuntSession_Returns403()
    {
        using var owner = await NewClient("hs-owner");
        using var other = await NewClient("hs-other");
        var characterId = await CreateCharacter(owner);

        (await owner.PostAsync("/api/game/enter", null)).EnsureSuccessStatusCode();

        var started = await owner.PostAsJsonAsync("/api/hunt-session/start", new { characterId });
        started.EnsureSuccessStatusCode();
        var huntSessionId = (await started.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("huntSessionId").GetGuid();

        var stop = await other.PostAsJsonAsync("/api/hunt-session/stop", new { huntSessionId });

        Assert.Equal(HttpStatusCode.Forbidden, stop.StatusCode);

        // Controle (ressalva da revisão do PV-QA-003): o dono continua conseguindo encerrar a sessão.
        Assert.Equal(
            HttpStatusCode.OK,
            (await owner.PostAsJsonAsync("/api/hunt-session/stop", new { huntSessionId })).StatusCode);
    }

    [Fact]
    public async Task OtherAccount_CannotAttackEncounter_Returns403()
    {
        using var owner = await NewClient("enc-owner");
        using var other = await NewClient("enc-other");
        var characterId = await CreateCharacter(owner);

        var enter = await owner.PostAsync("/api/game/enter", null);
        enter.EnsureSuccessStatusCode();
        var runtimeId = (await enter.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("runtimeId").GetGuid();

        var hunt = await owner.PostAsJsonAsync("/api/hunt/execute", new { characterId, runtimeId });
        hunt.EnsureSuccessStatusCode();
        var encounterId = (await hunt.Content.ReadFromJsonAsync<JsonElement>())
            .GetProperty("encounter").GetProperty("id").GetGuid();

        var attack = await other.PostAsJsonAsync("/api/hunt/attack", new { encounterId });

        Assert.Equal(HttpStatusCode.Forbidden, attack.StatusCode);

        // Controle: o ataque do dono funciona (o encounter continua dele e intacto).
        Assert.Equal(
            HttpStatusCode.OK,
            (await owner.PostAsJsonAsync("/api/hunt/attack", new { encounterId })).StatusCode);
    }

    [Fact]
    public async Task ExpiredToken_Returns401()
    {
        using var client = _factory.CreateClient();

        // Expirado há 10 minutos (acima da tolerância de relógio padrão de 5 minutos).
        var token = CreateToken(
            [new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())],
            DateTime.UtcNow.AddMinutes(-30),
            DateTime.UtcNow.AddMinutes(-10));

        var response = await ApiFactory.WithToken(client, token).GetAsync("/api/character");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ValidTokenFromSameHelper_IsAccepted()
    {
        // Controle do teste de token expirado (ressalva da revisão do PV-QA-003): o mesmo
        // gerador, com validade correta e a claim da conta, é aceito — então o 401 acima vem
        // do vencimento (tolerância padrão de 5 min do JwtBearer; rever se o ClockSkew mudar).
        using var client = _factory.CreateClient();

        var token = CreateToken(
            [new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())],
            DateTime.UtcNow.AddMinutes(-1),
            DateTime.UtcNow.AddMinutes(10));

        var response = await ApiFactory.WithToken(client, token).GetAsync("/api/character");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // PV-SEC-002 (R-018): token válido (assinatura, Issuer, Audience, validade) sem identidade de
    // conta utilizável → 401, inclusive em endpoint que não lê a identidade (playable/status).
    // Antes do PV-SEC-002 este caso respondia 500 nos endpoints que liam a identidade.
    public static TheoryData<string?, string> InvalidAccountClaims
    {
        get
        {
            var data = new TheoryData<string?, string>();

            foreach (var claim in new string?[] { null, "nao-e-um-guid", Guid.Empty.ToString() })
            {
                foreach (var url in new[] { "/api/character", "/api/playable/status" })
                {
                    data.Add(claim, url);
                }
            }

            return data;
        }
    }

    private static string TokenWithAccountClaim(string? accountClaim)
    {
        var claims = new List<Claim> { new(ClaimTypes.Name, "sem-conta") };
        if (accountClaim is not null)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, accountClaim));
        }

        return CreateToken(claims, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(10));
    }

    [Theory]
    [MemberData(nameof(InvalidAccountClaims))]
    public async Task TokenWithoutUsableAccountClaim_Returns401(string? accountClaim, string url)
    {
        using var client = _factory.CreateClient();

        var response = await ApiFactory.WithToken(client, TokenWithAccountClaim(accountClaim)).GetAsync(url);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PublicLogin_StillWorks_EvenWithTokenWithoutAccountClaim()
    {
        // Ressalva da revisão do PV-SEC-002: a falha de autenticação não afeta endpoints públicos.
        using var anonymous = _factory.CreateClient();
        var email = $"public-{Guid.NewGuid():N}@test.com";
        var body = new { email, password = "Senha123!" };
        (await anonymous.PostAsJsonAsync("/api/account", body)).EnsureSuccessStatusCode();

        using var client = _factory.CreateClient();
        var login = await ApiFactory.WithToken(client, TokenWithAccountClaim(null))
            .PostAsJsonAsync("/api/account/login", body);

        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
    }
}
