using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectVictory.API.Authentication;
using ProjectVictory.API.Middleware;
using ProjectVictory.API.Session;
using ProjectVictory.Application.Account;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Inventory;
using ProjectVictory.Application.Item;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Items;
using ProjectVictory.Infrastructure.Services;
using ProjectVictory.Persistence.Account;
using ProjectVictory.Persistence.Character;
using ProjectVictory.Persistence.Combat;
using ProjectVictory.Persistence.Database;
using ProjectVictory.Persistence.Database.Seed;
using ProjectVictory.Persistence.Hunt;
using ProjectVictory.Persistence.Item;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "ProjectVictoryClient",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:5173")
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

var jwtKey =
    builder.Configuration["Jwt:Key"]
    ?? "ProjectVictoryDevelopmentSecretKey123456";

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
                        Encoding.UTF8.GetBytes(jwtKey)),

                ValidateIssuer = false,

                ValidateAudience = false
            };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ProjectVictoryDbContext>(
    options =>
    {
        options.UseSqlite(
            "Data Source=projectvictory.db");
    });

builder.Services.AddScoped<
    IAccountRepository,
    AccountRepository>();

builder.Services.AddScoped<
    ICharacterRepository,
    CharacterRepository>();

builder.Services.AddSingleton<
    IHuntRepository,
    HuntRepository>();

builder.Services.AddSingleton<
    ICombatContextRepository,
    CombatContextRepository>();

builder.Services.AddScoped<
    IItemDefinitionRepository,
    ItemDefinitionRepository>();

builder.Services.AddScoped<
    IPasswordHasher,
    PasswordHasher>();

builder.Services.AddScoped<
    ICombatResolver,
    CombatResolver>();

builder.Services.AddScoped<
    IRewardProcessor,
    RewardProcessor>();

builder.Services.AddScoped<
    IProgressionProcessor,
    ProgressionProcessor>();

builder.Services.AddScoped<
    GetCharacterInventoryHandler>();

builder.Services.AddScoped<
    EncounterGenerator>();

builder.Services.AddSingleton<
    IEncounterStateStore,
    EncounterStateStore>();

builder.Services.AddSingleton<
    IRuntimeExecutionStateStore,
    RuntimeExecutionStateStore>();

builder.Services.AddScoped<
    CreateAccountHandler>();

builder.Services.AddScoped<
    LoginHandler>();

builder.Services.AddScoped<
    CreateCharacterHandler>();

builder.Services.AddScoped<
    GetCharactersHandler>();

builder.Services.AddScoped<
    EnterGameHandler>();

builder.Services.AddScoped<
    ExecuteHuntHandler>();

builder.Services.AddScoped<
    AttackEncounterHandler>();

builder.Services.AddScoped<
    GetItemDefinitionHandler>();

builder.Services.AddScoped<
    CreateRuntimeExecutionHandler>();

var app =
    builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context =
        scope.ServiceProvider
            .GetRequiredService<ProjectVictoryDbContext>();

    await context.Database.MigrateAsync();

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