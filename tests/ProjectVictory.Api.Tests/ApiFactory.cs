using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ProjectVictory.Api.Tests;

// PV-QA-001 parte 2: sobe a API real em memória (Development, banco SQLite temporário).
// A chave JWT é gerada aleatoriamente a cada execução — nunca há segredo em arquivo.
// Program.cs lê a configuração antes do Build(), por isso ela entra por variável de ambiente.
public sealed class ApiFactory : WebApplicationFactory<Program>
{
    public static readonly string JwtKey =
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));

    public const string Issuer = "ProjectVictory.API";

    public const string Audience = "ProjectVictory.Client";

    private readonly string _databaseDirectory =
        Path.Combine(Path.GetTempPath(), "pv-api-tests-" + Guid.NewGuid().ToString("N"));


    public ApiFactory()
    {
        Directory.CreateDirectory(_databaseDirectory);

        Environment.SetEnvironmentVariable("Jwt__Key", JwtKey);
        Environment.SetEnvironmentVariable(
            "ConnectionStrings__ProjectVictory",
            "Data Source=" + Path.Combine(_databaseDirectory, "api-tests.db"));
    }


    public async Task<string> RegisterAndLoginAsync(
        HttpClient client,
        string email)
    {
        var body = new { email, password = "Senha123!" };

        (await client.PostAsJsonAsync("/api/account", body)).EnsureSuccessStatusCode();

        var login =
            await client.PostAsJsonAsync("/api/account/login", body);

        login.EnsureSuccessStatusCode();

        var payload =
            await login.Content.ReadFromJsonAsync<LoginPayload>();

        return payload!.Token;
    }


    public static HttpClient WithToken(
        HttpClient client,
        string token)
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        return client;
    }


    protected override void Dispose(
        bool disposing)
    {
        base.Dispose(disposing);

        // Só a pasta temporária criada por este fixture.
        try
        {
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            Directory.Delete(_databaseDirectory, recursive: true);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }


    private sealed record LoginPayload(
        Guid AccountId,
        string Token);
}
