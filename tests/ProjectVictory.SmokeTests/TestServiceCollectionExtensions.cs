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
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Infrastructure.Services;
using ProjectVictory.Persistence.Account;
using ProjectVictory.Persistence.Character;
using ProjectVictory.Persistence.Combat;
using ProjectVictory.Persistence.Database;
using ProjectVictory.Persistence.Hunt;

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

        services.AddSingleton<ICombatContextRepository, CombatContextRepository>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<ICombatResolver, CombatResolver>();

        services.AddScoped<IRewardProcessor, TestRewardProcessor>();

        services.AddScoped<IProgressionProcessor, ProgressionProcessor>();

        // Account
        services.AddScoped<CreateAccountHandler>();
        services.AddScoped<LoginHandler>();

        // Character
        services.AddScoped<CreateCharacterHandler>();

        // Runtime
        services.AddScoped<CreateRuntimeExecutionHandler>();

        // Hunt
        services.AddScoped<ExecuteHuntHandler>();
        services.AddScoped<AttackEncounterHandler>();

        // Combat
        services.AddScoped<CreateCombatContextHandler>();
        services.AddScoped<CreateCombatEncounterHandler>();
        services.AddScoped<CreateGameplayActionHandler>();
        services.AddScoped<ExecuteCombatActionHandler>();
        services.AddScoped<GenerateCombatResolutionHandler>();
        services.AddScoped<ApplyCombatResultHandler>();
        services.AddScoped<FinalizeCombatHandler>();
        services.AddScoped<GenerateCombatActionResultHandler>();
        services.AddScoped<ExpandCombatResultHandler>();
        services.AddScoped<ProcessCombatResultHandler>();
        services.AddScoped<ReturnCombatControlHandler>();
        services.AddScoped<RefineCombatFlowHandler>();
        services.AddScoped<ValidateCombatContentHandler>();
        services.AddScoped<ValidateGameplayDepthHandler>();

        // Reward
        services.AddScoped<ApplyRewardHandler>();

        // Progression
        services.AddScoped<GenerateExperienceResultHandler>();
        services.AddScoped<ProcessExperienceRewardHandler>();

        return services;
    }
}