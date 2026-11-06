using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Supply;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.Runtime;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-SUPPLY-010: poções de vida e de mana (DEC-20261106-02/03/04).
public sealed class SupplyTests
{
    private static Combatant Player(int hp, int mana) =>
        new(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Player, "P", 1, 100, hp, 100, mana, new CombatProfile(10, 14, 4, 6, 10, 1.5m));

    private static CombatSession Session(Combatant player) =>
        new(Guid.NewGuid(), player,
            [new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Monster, "Rat", 1, 20, 20, 0, 0, new CombatProfile(10, 8, 1, 1))],
            new SeededRandomSource(1));

    [Theory]
    [InlineData(null, true)]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(101, false)]
    public void Policy_PercentValidation(int? value, bool valid)
    {
        Assert.Equal(valid, SupplyPolicy.IsValidPercent(value));
    }

    [Fact]
    public void PolicyStore_PendingOnlyBecomesActiveWhenActivated()
    {
        var store = new SupplyPolicyStore();
        var id = Guid.NewGuid();

        store.SetPending(id, new SupplyPolicy(50, null));

        Assert.Equal(SupplyPolicy.None, store.GetActive(id));            // não muda o turno em andamento
        Assert.Equal(50, store.ActivatePending(id).HealthPotionBelowPercent); // vale no próximo turno
        Assert.Equal(50, store.GetActive(id).HealthPotionBelowPercent);
    }

    [Fact]
    public void Selector_UsesHealthPotion_BelowThreshold_OnlyWithPotions()
    {
        var supplies = new CombatSupplyContext();
        var selector = new SupplyAwareCombatActionSelector(supplies);
        var hurt = Player(hp: 40, mana: 100);

        supplies.Load(new SupplyPolicy(50, null), healthPotions: 2, manaPotions: 0);
        Assert.Equal(CombatActionKind.UseHealthPotion, selector.SelectSecondaryAction(Session(hurt), hurt));

        supplies.Load(new SupplyPolicy(50, null), healthPotions: 0, manaPotions: 0);
        // Sem poção não há cura: cai na habilidade da classe (mana disponível).
        Assert.Equal(CombatActionKind.ClassAbility, selector.SelectSecondaryAction(Session(hurt), hurt));

        var healthy = Player(hp: 90, mana: 100);
        supplies.Load(new SupplyPolicy(50, null), healthPotions: 2, manaPotions: 0);
        Assert.Equal(CombatActionKind.ClassAbility, selector.SelectSecondaryAction(Session(healthy), healthy));
    }

    [Fact]
    public void Selector_UsesManaPotion_BelowThreshold()
    {
        var supplies = new CombatSupplyContext();
        var player = Player(hp: 100, mana: 5);

        supplies.Load(new SupplyPolicy(null, 30), healthPotions: 0, manaPotions: 1);

        Assert.Equal(CombatActionKind.UseManaPotion, new SupplyAwareCombatActionSelector(supplies).SelectSecondaryAction(Session(player), player));
    }

    [Fact]
    public void OutOfSupplies_OnlyForPotionsThePolicyUses()
    {
        var supplies = new CombatSupplyContext();

        supplies.Load(SupplyPolicy.None, 0, 0);
        Assert.False(supplies.IsOutOfSupplies);

        supplies.Load(new SupplyPolicy(50, null), 1, 0);
        Assert.False(supplies.IsOutOfSupplies);

        supplies.Consume(CombatActionKind.UseHealthPotion);
        Assert.True(supplies.IsOutOfSupplies);
        Assert.Equal(1, supplies.HealthPotionsUsed);
    }

    // --- integração: ataque real com poções do inventário ---

    private static ServiceProvider Provider(int seed, int starterHealthPotions)
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();

        var databaseName = $"supply-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
        services.AddSingleton(new CombatSettings(seed));
        services.AddSingleton(new SupplySettings(starterHealthPotions, 0));

        return services.BuildServiceProvider();
    }

    // PV-SUPPLY-011 (DEC-20261106-22): SUBSTITUI "fim de suprimento encerra a Hunt" (PV-SUPPLY-010).
    // As poções saem do inventário; sem poção o personagem segue sem cura; a Hunt só termina por
    // vitória do encounter (até o Hunt Loop do M6) ou por morte — nunca por fim de suprimento.
    [Fact]
    public async Task Potions_AreConsumedFromInventory_AndRunningOutDoesNotEndTheHunt()
    {
        await using var provider = Provider(seed: 11, starterHealthPotions: 2);
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand("S" + Guid.NewGuid().ToString("N")[..10], CharacterClass.Knight));
        var repository = scope.ServiceProvider.GetRequiredService<ICharacterRepository>();
        var enter = await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand());

        await scope.ServiceProvider.GetRequiredService<SetSupplyPolicyHandler>()
            .Handle(new SetSupplyPolicyCommand(characterId, 100, null));

        var hunt = await scope.ServiceProvider.GetRequiredService<ExecuteHuntHandler>()
            .Handle(new ExecuteHuntCommand(characterId, enter.RuntimeId));

        var rounds = new List<AttackEncounterResult>();
        for (var i = 0; i < 300 && rounds.LastOrDefault()?.CombatFinished != true; i++)
        {
            rounds.Add(await scope.ServiceProvider.GetRequiredService<AttackEncounterHandler>()
                .Handle(new AttackEncounterCommand(hunt.Encounter.Id)));
        }

        var potionTurns = rounds.SelectMany(r => r.Turns!).Count(t => t.Kind == nameof(CombatActionKind.UseHealthPotion));
        var potionsLeft = (await repository.GetByIdAsync(characterId))!.Inventory.Items
            .Count(i => i.ItemDefinitionId == SupplyCatalog.HealthPotionId);

        Assert.Equal(2, potionTurns);
        Assert.Equal(0, potionsLeft);

        // Acabaram as poções e a Hunt continuou: nenhuma rodada encerrou por fim de suprimento.
        Assert.DoesNotContain(rounds, r => r.HuntEndReason == nameof(HuntEndReason.OutOfSupplies));
        Assert.Contains(rounds[^1].HuntEndReason, new[] { nameof(HuntEndReason.Victory), nameof(HuntEndReason.Death) });
        Assert.True(rounds.Count(r => r.HuntEndReason is not null) == 1);
    }

    // PV-SUPPLY-011: SUBSTITUI "política ligada sem poção encerra na 1ª rodada" — agora a Hunt segue.
    [Fact]
    public async Task PolicyWithoutPotions_DoesNotEndTheHunt()
    {
        await using var provider = Provider(seed: 3, starterHealthPotions: 0);
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand("S" + Guid.NewGuid().ToString("N")[..10], CharacterClass.Knight));
        var enter = await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand());

        await scope.ServiceProvider.GetRequiredService<SetSupplyPolicyHandler>()
            .Handle(new SetSupplyPolicyCommand(characterId, 50, null));

        var hunt = await scope.ServiceProvider.GetRequiredService<ExecuteHuntHandler>()
            .Handle(new ExecuteHuntCommand(characterId, enter.RuntimeId));

        var result = await scope.ServiceProvider.GetRequiredService<AttackEncounterHandler>()
            .Handle(new AttackEncounterCommand(hunt.Encounter.Id));

        Assert.NotEqual(nameof(HuntEndReason.OutOfSupplies), result.HuntEndReason);
        Assert.NotEqual("Cancelled", result.CombatState);
    }
}
