using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Account;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Infrastructure.Services;
using ProjectVictory.Persistence.Account;
using ProjectVictory.Persistence.Character;
using ProjectVictory.Persistence.Database;
using ProjectVictory.Persistence.Hunt;
using ProjectVictory.Application.Hunt;

namespace ProjectVictory.SmokeTests;

public static class TestServiceCollectionExtensions
{
    public static IServiceCollection AddProjectVictorySmokeTestServices(
        this IServiceCollection services)
    {
        services.AddDbContext<ProjectVictoryDbContext>(
            options => options.UseInMemoryDatabase("ProjectVictorySmokeTest"));

        services.AddSingleton<TestCurrentPlayerContext>();
        services.AddScoped<EncounterGenerator>();

        services.AddSingleton<ICurrentPlayerContext>(
            provider => provider.GetRequiredService<TestCurrentPlayerContext>());

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICharacterRepository, CharacterRepository>();

        services.AddSingleton<IHuntRepository, HuntRepository>();
        services.AddSingleton<IEncounterStateStore, EncounterStateStore>();
        services.AddSingleton<IRuntimeExecutionStateStore, RuntimeExecutionStateStore>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ICombatResolver, CombatResolver>();
        services.AddScoped<IRewardProcessor, RewardProcessor>();
        services.AddScoped<IProgressionProcessor, ProgressionProcessor>();

        services.AddScoped<CreateAccountHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<CreateCharacterHandler>();
        services.AddScoped<CreateRuntimeExecutionHandler>();
        services.AddScoped<ExecuteHuntHandler>();
        services.AddScoped<AttackEncounterHandler>();

        return services;
    }
}
