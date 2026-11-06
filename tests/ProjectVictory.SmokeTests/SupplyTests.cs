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

    [Fact]
    public async Task Potions_AreConsumedFromInventory_AndOutOfSuppliesEndsHuntThroughPipeline()
    {
        await using var provider = Provider(seed: 11, starterHealthPotions: 2);
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand("S" + Guid.NewGuid().ToString("N")[..10], CharacterClass.Knight));
        var repository = scope.ServiceProvider.GetRequiredService<ICharacterRepository>();

        Assert.Equal(2, (await repository.GetByIdAsync(characterId))!.Inventory.Items.Count(i => i.ItemDefinitionId == SupplyCatalog.HealthPotionId));

        var enter = await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand());

        // Política: usar poção de vida abaixo de 100% (sempre que tiver tomado dano).
        await scope.ServiceProvider.GetRequiredService<SetSupplyPolicyHandler>()
            .Handle(new SetSupplyPolicyCommand(characterId, 100, null));

        var hunt = await scope.ServiceProvider.GetRequiredService<ExecuteHuntHandler>()
            .Handle(new ExecuteHuntCommand(characterId, enter.RuntimeId));

        AttackEncounterResult? last = null;
        for (var i = 0; i < 200 && last?.CombatFinished != true; i++)
        {
            last = await scope.ServiceProvider.GetRequiredService<AttackEncounterHandler>()
                .Handle(new AttackEncounterCommand(hunt.Encounter.Id));
        }

        var potionTurns = last!.Turns!.Count(t => t.Kind == nameof(CombatActionKind.UseHealthPotion));
        var potionsLeft = (await repository.GetByIdAsync(characterId))!.Inventory.Items
            .Count(i => i.ItemDefinitionId == SupplyCatalog.HealthPotionId);

        // Semente 11 fixa: o Knight toma dano, bebe as 2 poções (uma por turno, a partir do turno
        // seguinte ao dano) e a Hunt termina por fim de suprimento, pelo pipeline único.
        Assert.True(potionTurns > 0);
        Assert.Equal(0, potionsLeft);
        Assert.Equal(nameof(HuntEndReason.OutOfSupplies), last.HuntEndReason);
        Assert.True(last.ReturnedToCity);

        var huntState = (await scope.ServiceProvider.GetRequiredService<IHuntRepository>().GetByIdAsync(hunt.HuntId))!.State;
        Assert.Equal(HuntState.Completed, huntState);

        var runtime = scope.ServiceProvider.GetRequiredService<IRuntimeExecutionStateStore>().Get(enter.RuntimeId);
        Assert.Equal(RuntimeActivity.Idle, runtime.GameplayContext!.CurrentActivity);
    }

    [Fact]
    public async Task OutOfSupplies_WithPolicyAndNoPotions_EndsAfterFirstRound()
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

        Assert.Equal(nameof(HuntEndReason.OutOfSupplies), result.HuntEndReason);
        Assert.True(result.CombatFinished);
        Assert.Equal("Cancelled", result.CombatState);
    }
}
