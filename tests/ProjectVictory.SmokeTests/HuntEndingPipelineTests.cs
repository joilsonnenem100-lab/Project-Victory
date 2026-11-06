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

            // PV-HUNT-011 (PV-STATE-001 D2): a Hunt encerrada sai da memória (antes ficava como Failed).
            Assert.Null(await scope.ServiceProvider.GetRequiredService<IHuntRepository>().GetByIdAsync(huntId));
            Assert.False(scope.ServiceProvider.GetRequiredService<IEncounterStateStore>().Contains(last.EncounterId));

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
    // PV-HUNT-011 (DEC-20261106-22, fecha o R-036): SUBSTITUI "a vitória do encounter encerra a Hunt".
    // Agora a vitória gera o PRÓXIMO encounter; a Hunt termina pela saída manual, pelo MESMO pipeline,
    // e nada é perdido.
    public async Task Victory_OfEncounter_ContinuesTheHunt_ManualLeaveEndsThroughSamePipeline_AndKeepsRewards()
    {
        await using var provider = BuildProvider(7);
        using var scope = provider.CreateScope();
        var (characterId, runtimeId) = await NewPlayer(scope, CharacterClass.Knight);
        var store = scope.ServiceProvider.GetRequiredService<IEncounterStateStore>();

        var runtimeBefore = scope.ServiceProvider.GetRequiredService<IRuntimeExecutionStateStore>().Get(runtimeId);
        runtimeBefore.CharacterState!.UpdateHealth(185);
        var started = await scope.ServiceProvider.GetRequiredService<ExecuteHuntHandler>()
            .Handle(new ExecuteHuntCommand(characterId, runtimeId));
        var monsters = store.Get(started.Encounter.Id).Monsters.Select(m => m.MonsterId).ToList();

        AttackEncounterResult? last = null;
        for (var i = 0; i < 200 && last?.CombatFinished != true; i++)
        {
            last = await scope.ServiceProvider.GetRequiredService<AttackEncounterHandler>()
                .Handle(new AttackEncounterCommand(started.Encounter.Id));
        }

        // Vitória do encounter: a Hunt continua, com o próximo encounter já gerado pelo Spawn.
        Assert.Equal("Victory", last!.CombatState);
        Assert.Null(last.HuntEndReason);
        Assert.False(last.ReturnedToCity);
        Assert.NotNull(last.NextEncounterId);
        Assert.False(store.Contains(started.Encounter.Id));      // o vencido foi descartado
        Assert.True(store.Contains(last.NextEncounterId!.Value));
        var huntId = started.HuntId;
        Assert.Equal(HuntState.Running, (await scope.ServiceProvider.GetRequiredService<IHuntRepository>().GetByIdAsync(huntId))!.State);

        // XP do encounter vencido = soma do XP dos monstros (catálogo).
        var expectedXp = monsters.Sum(id => (long)ProjectVictory.Infrastructure.Content.SpiderCaveCatalog.Find(id)!.Loot.Experience);
        Assert.Equal(expectedXp, last.HuntExperienceGained);

        // Saída manual: mesmo pipeline (volta à cidade, aguarda comando), nada é retirado.
        var ending = await scope.ServiceProvider.GetRequiredService<LeaveHuntHandler>().Handle(new LeaveHuntCommand(huntId));
        Assert.Equal(HuntEndReason.Manual, ending.Reason);
        Assert.True(ending.ReturnedToCity);
        Assert.Equal(expectedXp, ending.ExperienceGained);

        var runtime = scope.ServiceProvider.GetRequiredService<IRuntimeExecutionStateStore>().Get(runtimeId);
        Assert.Equal(RuntimeActivity.Idle, runtime.GameplayContext!.CurrentActivity);
        Assert.Equal(EndHuntHandler.MainCity, runtime.WorldContext);
        Assert.Null(await scope.ServiceProvider.GetRequiredService<IHuntRepository>().GetByIdAsync(huntId));
        Assert.False(store.Contains(last.NextEncounterId.Value));  // o encounter em andamento também saiu

        var character = await scope.ServiceProvider.GetRequiredService<ICharacterRepository>().GetByIdAsync(characterId);
        Assert.Equal(expectedXp, character!.Experience.Value);
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
