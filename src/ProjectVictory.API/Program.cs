using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectVictory.API.Authentication;
using ProjectVictory.API.Session;
using ProjectVictory.Application.Account;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Infrastructure.Services;
using ProjectVictory.Persistence.Account;
using ProjectVictory.Persistence.Character;
using ProjectVictory.Persistence.Database;
using ProjectVictory.Persistence.Hunt;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


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


// JWT

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


// Persistence

builder.Services.AddDbContext<ProjectVictoryDbContext>(
    options =>
    {
        options.UseSqlite(
            "Data Source=projectvictory.db");
    });


builder.Services.AddScoped<
    IAccountRepository,
    AccountRepository>();

builder.Services.AddSingleton<
    ICharacterRepository,
    CharacterRepository>();

builder.Services.AddScoped<
    IHuntRepository,
    HuntRepository>();


// Infrastructure

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


// Application

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


var app = builder.Build();


app.UseCors(
    "ProjectVictoryClient");


app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();


app.Run();