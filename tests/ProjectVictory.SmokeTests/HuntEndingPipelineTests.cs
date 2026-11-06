using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Runtime;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-HUNT-010: vitória e morte encerram a Hunt pelo MESMO pipeline (ADR-012); o jogador volta à
// cidade, aguarda comando, e nada é perdido (DEC-20261106-04). Na morte, HP e Mana cheios
// (knowledge-base/character.md).
public sealed class HuntEndingPipelineTests
{
    private static ServiceProvider BuildProvider(int seed)
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();

        var databaseName = $"hunt-end-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
        services.AddSingleton(new CombatSettings(seed));

        return services.BuildServiceProvider();
    }

    private static async Task<(Guid CharacterId, Guid RuntimeId)> NewPlayer(IServiceScope scope, CharacterClass characterClass)
    {
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand("H" + Guid.NewGuid().ToString("N")[..10], characterClass));

        var enter = await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand());

        return (characterId, enter.RuntimeId);
    }

    private static async Task<(AttackEncounterResult Last, Guid HuntId)> FightToTheEnd(IServiceScope scope, Guid characterId, Guid runtimeId, int startingHealth)
    {
        var runtime = scope.ServiceProvider.GetRequiredService<IRuntimeExecutionStateStore>().Get(runtimeId);
        runtime.CharacterState!.UpdateHealth(startingHealth);

        var hunt = await scope.ServiceProvider.GetRequiredService<ExecuteHuntHandler>()
            .Handle(new ExecuteHuntCommand(characterId, runtimeId));

        AttackEncounterResult? last = null;
        for (var i = 0; i < 200 && last?.CombatFinished != true; i++)
        {
            last = await scope.ServiceProvider.GetRequiredService<AttackEncounterHandler>()
                .Handle(new AttackEncounterCommand(hunt.Encounter.Id));
        }

        return (last!, hunt.HuntId);
    }

    [Fact]
    public async Task Death_EndsHuntThroughPipeline_ReturnsToCityWithFullHealth_KeepsXp()
    {
        // Jogador com 1 de vida: com semente fixa, procura uma semente em que ele morra antes de
        // vencer. A busca é sobre sementes fixas, então o teste é determinístico.
        for (var seed = 1; seed <= 30; seed++)
        {
            await using var provider = BuildProvider(seed);
            using var scope = provider.CreateScope();
            var (characterId, runtimeId) = await NewPlayer(scope, CharacterClass.Sorcerer);

            var (last, huntId) = await FightToTheEnd(scope, characterId, runtimeId, startingHealth: 1);

            if (last.HuntEndReason != nameof(HuntEndReason.Death))
            {
                continue;
            }

            Assert.True(last.PlayerDefeated);
            Assert.True(last.ReturnedToCity);

            var hunt = await scope.ServiceProvider.GetRequiredService<IHuntRepository>().GetByIdAsync(huntId);
            Assert.Equal(HuntState.Failed, hunt!.State);

            var runtime = scope.ServiceProvider.GetRequiredService<IRuntimeExecutionStateStore>().Get(runtimeId);
            var character = await scope.ServiceProvider.GetRequiredService<ICharacterRepository>().GetByIdAsync(characterId);

            Assert.Equal(RuntimeActivity.Idle, runtime.GameplayContext!.CurrentActivity);
            Assert.Equal(EndHuntHandler.MainCity, runtime.WorldContext);
            Assert.Equal(character!.MaximumHealth.Value, runtime.CharacterState!.CurrentHealth);
            Assert.Equal(character.MaximumMana.Value, runtime.CharacterState.CurrentMana);
            return;
        }

        Assert.Fail("Nenhuma semente entre 1 e 30 produziu morte com 1 de vida.");
    }

    [Fact]
    public async Task Victory_EndsHuntThroughSamePipeline_AndKeepsRewards()
    {
        await using var provider = BuildProvider(7);
        using var scope = provider.CreateScope();
        var (characterId, runtimeId) = await NewPlayer(scope, CharacterClass.Knight);

        var (last, huntId) = await FightToTheEnd(scope, characterId, runtimeId, startingHealth: 185);

        Assert.Equal(nameof(HuntEndReason.Victory), last.HuntEndReason);
        Assert.True(last.ReturnedToCity);

        var hunt = await scope.ServiceProvider.GetRequiredService<IHuntRepository>().GetByIdAsync(huntId);
        Assert.Equal(HuntState.Completed, hunt!.State);

        var runtime = scope.ServiceProvider.GetRequiredService<IRuntimeExecutionStateStore>().Get(runtimeId);
        Assert.Equal(RuntimeActivity.Idle, runtime.GameplayContext!.CurrentActivity);
        Assert.Equal(EndHuntHandler.MainCity, runtime.WorldContext);

        // Rats mortos: 10 XP cada (Cave Rat ainda sem definição até o PV-CONTENT-011); nada é retirado.
        var character = await scope.ServiceProvider.GetRequiredService<ICharacterRepository>().GetByIdAsync(characterId);
        Assert.Equal(20, character!.Experience.Value);
    }

    [Fact]
    public async Task Death_DoesNotRemoveExperienceAlreadyGained()
    {
        await using var provider = BuildProvider(7);
        using var scope = provider.CreateScope();
        var (characterId, runtimeId) = await NewPlayer(scope, CharacterClass.Knight);

        var hunt = await scope.ServiceProvider.GetRequiredService<ExecuteHuntHandler>()
            .Handle(new ExecuteHuntCommand(characterId, runtimeId));

        var repository = scope.ServiceProvider.GetRequiredService<ICharacterRepository>();
        var before = await repository.GetByIdAsync(characterId);
        before!.GainExperience(new ProjectVictory.Domain.ValueObjects.Experience(500));
        await repository.UpdateAsync(before);

        await scope.ServiceProvider.GetRequiredService<EndHuntHandler>()
            .Handle(new EndHuntCommand(hunt.HuntId, HuntEndReason.Death));

        var after = await repository.GetByIdAsync(characterId);
        Assert.Equal(500, after!.Experience.Value);
    }
}
