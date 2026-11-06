using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

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


    private readonly string? _previousJwtKey =
        Environment.GetEnvironmentVariable("Jwt__Key");

    private readonly string? _previousConnectionString =
        Environment.GetEnvironmentVariable("ConnectionStrings__ProjectVictory");


    public ApiFactory()
    {
        Directory.CreateDirectory(_databaseDirectory);

        Environment.SetEnvironmentVariable("Jwt__Key", JwtKey);
        Environment.SetEnvironmentVariable(
            "ConnectionStrings__ProjectVictory",
            "Data Source=" + Path.Combine(_databaseDirectory, "api-tests.db"));
    }


    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        // Explícito: o MigrateAsync() do startup só roda em Development (PV-PLATFORM-001).
        builder.UseEnvironment("Development");
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

        Environment.SetEnvironmentVariable("Jwt__Key", _previousJwtKey);
        Environment.SetEnvironmentVariable("ConnectionStrings__ProjectVictory", _previousConnectionString);

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


// Todas as classes que sobem a API compartilham uma única instância (e não rodam em paralelo),
// porque a configuração entra por variáveis de ambiente do processo.
[CollectionDefinition(Name)]
public sealed class ApiCollection : ICollectionFixture<ApiFactory>
{
    public const string Name = "Api";
}
