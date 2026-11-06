using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
using ProjectVictory.Composition;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.Reward;
using ProjectVictory.Persistence.Database;

namespace ProjectVictory.SmokeTests;

public static class TestServiceCollectionExtensions
{
    public static IServiceCollection AddProjectVictorySmokeTestServices(
        this IServiceCollection services)
    {
        // Partes próprias do host de teste (na API: SQLite e identidade vinda do token).
        services.AddDbContext<ProjectVictoryDbContext>(
            options => options.UseInMemoryDatabase("ProjectVictorySmokeTest"));

        services.AddSingleton<TestCurrentPlayerContext>();

        services.AddSingleton<ICurrentPlayerContext>(
            provider => provider.GetRequiredService<TestCurrentPlayerContext>());

        // PV-ARCH-001 (R-014): a MESMA composição usada pelo Program.cs da API.
        // Antes este arquivo replicava os registros à mão (já defasou duas vezes).
        services.AddProjectVictoryCore(
            new RewardSettings(ExperienceRate.Default));

        // Dublês de teste (o último registro vence na resolução).
        services.AddScoped<IRewardProcessor, TestRewardProcessor>();
        services.AddSingleton<IRandomSource>(new FixedRandomSource());

        // Handlers usados só pelos testes: existem em src/ mas não são registrados pela API.
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
        services.AddScoped<ApplyRewardHandler>();
        services.AddScoped<GenerateExperienceResultHandler>();
        services.AddScoped<ProcessExperienceRewardHandler>();

        return services;
    }
}
