using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectVictory.API.Authentication;
using ProjectVictory.API.Database;
using ProjectVictory.API.Middleware;
using ProjectVictory.API.Session;
using ProjectVictory.Composition;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
using ProjectVictory.Persistence.Database;
using ProjectVictory.Persistence.Database.Seed;
using System.Text;
using System.Text.Json.Serialization;

var builder =
    WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

// PV-PLATFORM-002: origens CORS vêm de configuração (Cors:AllowedOrigins).
// Lista vazia = nenhuma origem liberada (a API sobe, o navegador é bloqueado).
var allowedOrigins =
    builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>()
    ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "ProjectVictoryClient",
        policy =>
        {
            policy
                .WithOrigins(
                    allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<
    ICurrentPlayerContext,
    HttpCurrentPlayerContext>();

builder.Services.AddScoped<
    JwtTokenGenerator>();

builder.Services.AddExceptionHandler<ApiExceptionHandler>();

builder.Services.AddProblemDetails();

// PV-PLATFORM-002: chave, Issuer e Audience vêm de configuração, sem valor padrão.
// Falta ou chave fraca derruba o startup com mensagem clara (JwtSettings.Validate).
var jwtSettings =
    builder.Configuration
        .GetSection(JwtSettings.SectionName)
        .Get<JwtSettings>()
    ?? new JwtSettings();

jwtSettings.Validate(
    builder.Environment.IsDevelopment());

// A mesma instância validada serve à emissão (JwtTokenGenerator) e à validação (JwtBearer).
builder.Services.AddSingleton(
    Microsoft.Extensions.Options.Options.Create(jwtSettings));

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key)),

                ValidateIssuer = true,

                ValidIssuer = jwtSettings.Issuer,

                ValidateAudience = true,

                ValidAudience = jwtSettings.Audience
            };

        // PV-SEC-002 (R-018): token válido sem a claim da conta (ou com valor que não é Guid)
        // não autentica → 401 em todo endpoint [Authorize], em vez de 500 adiante.
        options.Events =
            new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    if (!AccountClaim.TryGetAccountId(context.Principal, out _))
                    {
                        context.Fail("Token sem a claim da conta.");
                    }

                    return Task.CompletedTask;
                }
            };
    });

builder.Services.AddAuthorization();

// PV-PLATFORM-002: connection string vem de configuração (ConnectionStrings:ProjectVictory).
var connectionString =
    builder.Configuration.GetConnectionString("ProjectVictory");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "ConnectionStrings:ProjectVictory não configurada — a API não pode subir. "
        + "Defina em appsettings.json ou pela variável de ambiente ConnectionStrings__ProjectVictory. "
        + "Ver developer-guide/guides/configuracao-local.md.");
}

builder.Services.AddDbContext<ProjectVictoryDbContext>(
    options =>
    {
        options.UseSqlite(
            connectionString);

        // PV-PLATFORM-002: log de SQL com dados sensíveis só em Development.
        if (builder.Environment.IsDevelopment())
        {
            options
                .EnableSensitiveDataLogging()
                .LogTo(
                    Console.WriteLine,
                    LogLevel.Information);
        }
    });

// PV-ARCH-001 (R-014): todos os serviços do jogo vêm do método único de composição,
// o mesmo usado pelos testes (src/ProjectVictory.Composition). Aqui ficam só a
// configuração do servidor e o que é próprio da API (HTTP, JWT, CORS, DbContext).
//
// PV-REWARD-010: XP rate só do servidor (DEC-20261106-12), padrão 1x; valor inválido
// (<= 0 ou não numérico) derruba o startup. Nenhum endpoint lê ou altera este valor.
var experienceRateValue =
    builder.Configuration.GetValue<decimal?>(RewardSettings.ExperienceRateKey) ?? 1m;

// PV-COMBAT-011: "Combat:FixedSeed" fixa a semente dos combates — só vale em Development
// (testes/depuração). Fora dele é IGNORADA (revisão do PV-COMBAT-011: semente fixa em produção
// tornaria todo combate previsível).
var fixedCombatSeed =
    builder.Configuration.GetValue<int?>(ProjectVictory.Application.Combat.TurnBased.CombatSettings.FixedSeedKey);

if (fixedCombatSeed is not null && !builder.Environment.IsDevelopment())
{
    Console.Error.WriteLine(
        $"[aviso] {ProjectVictory.Application.Combat.TurnBased.CombatSettings.FixedSeedKey} ignorado fora de Development.");
    fixedCombatSeed = null;
}

builder.Services.AddProjectVictoryCore(
    new RewardSettings(
        new ProjectVictory.Domain.Reward.ExperienceRate(experienceRateValue)),
    new ProjectVictory.Application.Combat.TurnBased.CombatSettings(fixedCombatSeed),
    // PV-SUPPLY-010: poções iniciais (PROVISÓRIO; padrão 0 = nenhuma).
    new ProjectVictory.Application.Supply.SupplySettings(
        builder.Configuration.GetValue<int?>(ProjectVictory.Application.Supply.SupplySettings.StarterHealthPotionsKey) ?? 0,
        builder.Configuration.GetValue<int?>(ProjectVictory.Application.Supply.SupplySettings.StarterManaPotionsKey) ?? 0));

var app =
    builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context =
        scope.ServiceProvider
            .GetRequiredService<ProjectVictoryDbContext>();

    // PV-PLATFORM-001: em Development, aplica as migrations existentes antes do seed,
    // para que um clone limpo suba com o schema criado.
    // PV-PLATFORM-004: fora de Development, só aplica se a chave explícita
    // "Database:ApplyMigrationsOnStartup" estiver ligada (padrão: desligada, appsettings.json).
    var applyMigrationsOutsideDevelopment =
        app.Configuration.GetValue<bool>(DatabaseStartupSettings.ApplyMigrationsOnStartupKey);

    if (DatabaseStartupSettings.ShouldApplyMigrations(
            app.Environment.IsDevelopment(),
            applyMigrationsOutsideDevelopment))
    {
        await context.Database.MigrateAsync();
    }
    else
    {
        var pending =
            (await context.Database.GetPendingMigrationsAsync()).ToList();

        if (pending.Count > 0)
        {
            app.Logger.LogWarning(
                "Há {Count} migration(s) pendente(s) e a aplicação automática está desligada fora de Development. Aplique-as no deploy (dotnet ef database update / bundle) ou ligue {Key}=true. Pendentes: {Pending}",
                pending.Count,
                DatabaseStartupSettings.ApplyMigrationsOnStartupKey,
                string.Join(", ", pending));
        }
    }

    await ItemDefinitionSeed.ExecuteAsync(
        context);
}

app.UseExceptionHandler();

app.UseCors(
    "ProjectVictoryClient");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();