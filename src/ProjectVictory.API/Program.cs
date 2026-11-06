using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectVictory.API.Authentication;
using ProjectVictory.API.Middleware;
using ProjectVictory.API.Session;
using ProjectVictory.Application.Encounter;
using ProjectVictory.Application.Account;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.Gameplay;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Inventory;
using ProjectVictory.Application.Item;
using ProjectVictory.Application.Loot;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Items;
using ProjectVictory.Domain.Loot;
using ProjectVictory.Infrastructure.Loot;
using ProjectVictory.Infrastructure.Services;
using ProjectVictory.Persistence.Account;
using ProjectVictory.Persistence.Character;
using ProjectVictory.Persistence.Combat;
using ProjectVictory.Persistence.Database;
using ProjectVictory.Persistence.Database.Seed;
using ProjectVictory.Persistence.Hunt;
using ProjectVictory.Persistence.Item;
using ProjectVictory.Persistence.Runtime;
using System.Text;
using System.Text.Json.Serialization;
using ProjectVictory.Application.Runtime;
using ProjectVictory.Application.World;
using ProjectVictory.Domain.World;

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

// PV-SEC-001: verificação de posse de recursos do jogador.
builder.Services.AddScoped<PlayerOwnershipGuard>();

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

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IRuntimeRepository, RuntimeRepository>();
builder.Services.AddSingleton<IHuntRepository, HuntRepository>();
builder.Services.AddSingleton<ICombatContextRepository, CombatContextRepository>();
builder.Services.AddScoped<IItemDefinitionRepository, ItemDefinitionRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ICombatResolver, CombatResolver>();
builder.Services.AddScoped<IRewardProcessor, RewardProcessor>();

// PV-REWARD-010: XP rate só do servidor (DEC-20261106-12), padrão 1x; valor inválido
// (<= 0 ou não numérico) derruba o startup. Nenhum endpoint lê ou altera este valor.
var experienceRateValue =
    builder.Configuration.GetValue<decimal?>(RewardSettings.ExperienceRateKey) ?? 1m;

builder.Services.AddSingleton(
    new RewardSettings(
        new ProjectVictory.Domain.Reward.ExperienceRate(experienceRateValue)));

builder.Services.AddSingleton<
    ProjectVictory.Domain.Primitives.IRandomSource,
    SystemRandomSource>();
builder.Services.AddScoped<IProgressionProcessor, ProgressionProcessor>();
builder.Services.AddScoped<ICombatResultStore, CombatResultStore>();

builder.Services.AddSingleton<
    ILootDefinitionProvider,
    MonsterLootDefinitionProvider>();

builder.Services.AddScoped<LootResolutionHandler>();

builder.Services.AddScoped<ApplyLootToCharacterInventoryHandler>();

builder.Services.AddScoped<GetCharacterInventoryHandler>();
builder.Services.AddScoped<ApplyResolvedRewardToCharacterHandler>();

builder.Services.AddSingleton<IEncounterStateStore, EncounterStateStore>();

builder.Services.AddSingleton<
    IRuntimeExecutionStateStore,
    RuntimeExecutionStateStore>();

builder.Services.AddScoped<EncounterGenerator>();

builder.Services.AddScoped<CreateAccountHandler>();
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<CreateCharacterHandler>();
builder.Services.AddScoped<GetCharactersHandler>();
builder.Services.AddScoped<EnterGameHandler>();
builder.Services.AddScoped<ExecuteHuntHandler>();

builder.Services.AddScoped<IHuntSessionRepository, HuntSessionRepository>();
builder.Services.AddScoped<StartHuntSessionHandler>();
builder.Services.AddScoped<StopHuntSessionHandler>();

builder.Services.AddScoped<FinalizeHuntAfterCombatHandler>();
builder.Services.AddScoped<ForwardHuntRewardHandler>();
builder.Services.AddScoped<CompleteHuntRewardFlowHandler>();
builder.Services.AddScoped<RewardResolutionHandler>();
builder.Services.AddScoped<GameplayResultPipeline>();
builder.Services.AddScoped<AttackEncounterHandler>();
builder.Services.AddScoped<GetItemDefinitionHandler>();

builder.Services.AddScoped<CreateRuntimeExecutionHandler>();
builder.Services.AddScoped<UpdateRuntimeActivityHandler>();
builder.Services.AddScoped<ChangeRuntimeLocationHandler>();
builder.Services.AddScoped<ResolveLocationActionsHandler>();
builder.Services.AddScoped<BindEncounterToCombatHandler>();
builder.Services.AddScoped<StartEncounterCombatHandler>();
builder.Services.AddScoped<CreateCombatContextHandler>();
builder.Services.AddScoped<ResolveCombatHandler>();
builder.Services.AddScoped<GameplayActionDispatcher>();
builder.Services.AddScoped<GameplayActionExecutionHandler>();
builder.Services.AddSingleton<ILocationActionCatalog, LocationActionCatalog>();
builder.Services.AddSingleton<IWorldDefinitionStore, WorldDefinitionStore>();

var worldSeed =
    WorldDefinitionSeed.Create();

builder.Services.AddSingleton<IWorldDefinitionCatalog>(
    new WorldDefinitionCatalog(
        worldSeed.Worlds,
        worldSeed.Regions,
        worldSeed.Locations));

var app =
    builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context =
        scope.ServiceProvider
            .GetRequiredService<ProjectVictoryDbContext>();

    // PV-PLATFORM-001: em Development, aplica as migrations existentes antes do seed,
    // para que um clone limpo suba com o schema criado. Fora de Development nada muda.
    if (app.Environment.IsDevelopment())
    {
        await context.Database.MigrateAsync();
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