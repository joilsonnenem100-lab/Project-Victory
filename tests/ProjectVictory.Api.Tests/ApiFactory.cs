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

    // Revisão do PV-QA-006 (ressalva 3): senha única dos testes (não é segredo: só existe no banco temporário).
    public const string TestPassword = "Senha123!";

    private readonly string _databaseDirectory;

    // Ressalva R1 do PV-ADMIN-010: um segundo servidor pode reutilizar o banco de outro fixture
    // (ex.: subir de novo com Admin:Accounts = Id da conta já cadastrada); só o dono apaga a pasta.
    private readonly bool _ownsDatabaseDirectory;


    private readonly string? _previousJwtKey =
        Environment.GetEnvironmentVariable("Jwt__Key");

    private readonly string? _previousConnectionString =
        Environment.GetEnvironmentVariable("ConnectionStrings__ProjectVictory");


    private readonly string _environment;


    // PV-PLATFORM-004: caminho do banco SQLite temporário deste fixture.
    public string DatabasePath =>
        Path.Combine(_databaseDirectory, "api-tests.db");


    // xUnit cria o fixture pelo construtor sem parâmetros (Development).
    public ApiFactory()
        : this("Development")
    {
    }


    // PV-PLATFORM-004: permite subir a API em outro ambiente (ex.: Production).
    internal ApiFactory(string environment)
        : this(environment, Path.Combine(Path.GetTempPath(), "pv-api-tests-" + Guid.NewGuid().ToString("N")), ownsDatabaseDirectory: true)
    {
    }


    // Ressalva R1 do PV-ADMIN-010: mesmo banco SQLite de outro fixture (que continua dono da pasta).
    internal ApiFactory(ApiFactory sharingDatabaseWith)
        : this(sharingDatabaseWith._environment, sharingDatabaseWith._databaseDirectory, ownsDatabaseDirectory: false)
    {
    }


    private ApiFactory(string environment, string databaseDirectory, bool ownsDatabaseDirectory)
    {
        _environment = environment;
        _databaseDirectory = databaseDirectory;
        _ownsDatabaseDirectory = ownsDatabaseDirectory;

        Directory.CreateDirectory(_databaseDirectory);

        Environment.SetEnvironmentVariable("Jwt__Key", JwtKey);
        Environment.SetEnvironmentVariable(
            "ConnectionStrings__ProjectVictory",
            "Data Source=" + Path.Combine(_databaseDirectory, "api-tests.db"));
    }


    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        // Explícito: o MigrateAsync() do startup roda sempre em Development (PV-PLATFORM-001)
        // e, fora dele, só com Database:ApplyMigrationsOnStartup ligado (PV-PLATFORM-004).
        builder.UseEnvironment(_environment);
    }


    public async Task<string> RegisterAndLoginAsync(
        HttpClient client,
        string email) =>
        (await RegisterAndLoginWithAccountAsync(client, email)).Token;


    // Ressalva R1 do PV-ADMIN-010: devolve também o Id da conta (o login já o informa).
    public async Task<(Guid AccountId, string Token)> RegisterAndLoginWithAccountAsync(
        HttpClient client,
        string email)
    {
        var body = new { email, password = TestPassword };

        (await client.PostAsJsonAsync("/api/account", body)).EnsureSuccessStatusCode();

        return await LoginWithAccountAsync(client, email);
    }


    // Ressalva R1 do PV-ADMIN-010: só login (conta já cadastrada, ex.: em banco compartilhado).
    public async Task<(Guid AccountId, string Token)> LoginWithAccountAsync(
        HttpClient client,
        string email)
    {
        var body = new { email, password = TestPassword };

        var login =
            await client.PostAsJsonAsync("/api/account/login", body);

        login.EnsureSuccessStatusCode();

        var payload =
            await login.Content.ReadFromJsonAsync<LoginPayload>();

        return (payload!.AccountId, payload.Token);
    }


    // PV-CONTENT-012: o appsettings.json agora dá um kit inicial de poções (PO-16). Cenários antigos
    // que contam itens do inventário ou poções exatas (escritos antes do kit) sobem o servidor SEM o
    // kit — mesmo cenário de antes; as chaves antigas (StarterHealthPotions/ManaPotions) continuam.
    public static IDisposable WithoutStarterKit() => new StarterKitOff();


    private sealed class StarterKitOff : IDisposable
    {
        private readonly Dictionary<string, string?> _previous = new();

        public StarterKitOff()
        {
            foreach (var potion in ProjectVictory.Application.Supply.PotionCatalog.All)
            {
                var name = "Supplies__StarterKit__" + potion.Key;
                _previous[name] = Environment.GetEnvironmentVariable(name);
                Environment.SetEnvironmentVariable(name, "0");
            }
        }

        public void Dispose()
        {
            foreach (var (name, value) in _previous)
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }
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
        if (!_ownsDatabaseDirectory)
        {
            return;
        }

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
