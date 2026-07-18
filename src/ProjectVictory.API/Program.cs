using ProjectVictory.Application.Account;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Infrastructure.Services;
using ProjectVictory.Persistence.Account;
using ProjectVictory.Persistence.Character;
using ProjectVictory.Persistence.Hunt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Persistence
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IHuntRepository, HuntRepository>();

// Infrastructure Services
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ICombatResolver, CombatResolver>();
builder.Services.AddScoped<IRewardProcessor, RewardProcessor>();
builder.Services.AddScoped<IProgressionProcessor, ProgressionProcessor>();

// Application Handlers
builder.Services.AddScoped<CreateAccountHandler>();
builder.Services.AddScoped<CreateCharacterHandler>();
builder.Services.AddScoped<EnterGameHandler>();
builder.Services.AddScoped<ExecuteHuntHandler>();

var app = builder.Build();

app.MapControllers();

app.Run();