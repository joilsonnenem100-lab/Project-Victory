using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Application.Supply;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Infrastructure.Combat;
using ProjectVictory.Infrastructure.Services;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-COMBAT-014 (PO-53): spell de cura com o CombatResolver REAL, o seletor automático e o orquestrador.
public sealed class HealingSpellCombatTests
{
    private static readonly HealingSpellDefinition KnightHeal = ClassHealingSpellTable.ForClass(CharacterClass.Knight)!;

    private static Combatant Player(int level = 1, int maxHealth = 200, int health = 50, int mana = 100) =>
        new(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Player, "P", level, maxHealth, health, 100, mana,
            new CombatProfile(10, 14, 4, 6), [], KnightHeal);

    private static Combatant Monster(int health = 1000) =>
        new(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Monster, "M", 1, health, health, 0, 0, new CombatProfile(5, 5, 0, 0));

    private static CombatSession Session(Combatant player, int seed = 1) =>
        new(Guid.NewGuid(), player, [Monster()], new SeededRandomSource(seed));

    private static CombatResolver Resolver() => new(new ProvisionalTibiaLikeDamageFormula(), new ProvisionalSupplyEffects());

    private static SupplyAwareCombatActionSelector Selector(CombatSupplyContext supplies, HealingSpellPolicy? policy = null) =>
        new(supplies, new DataDrivenMonsterAI(BasicAttackOnlyBehaviorProvider.Instance), policy ?? HealingSpellPolicy.Default);

    private sealed class CountingRandom : IRandomSource
    {
        public int Calls { get; private set; }

        public double NextDouble()
        {
            Calls++;
            return 0.5;
        }

        public int NextInclusive(int minInclusive, int maxInclusive)
        {
            Calls++;
            return minInclusive;
        }
    }

    [Fact]
    public void Resolver_HealsPercentOfMaximumHealth_ByLevelBand_AndCostsMana_NoRandom()
    {
        var random = new CountingRandom();
        var low = Player(level: 1, maxHealth: 200, health: 20);
        var high = Player(level: 50, maxHealth: 200, health: 20);

        var lowOutcome = Resolver().ResolveAction(new CombatTurnAction(low, low, CombatActionKind.UseHealingSpell), random);
        var highOutcome = Resolver().ResolveAction(new CombatTurnAction(high, high, CombatActionKind.UseHealingSpell), random);

        Assert.True(lowOutcome.Executed);
        Assert.Equal(20, lowOutcome.Healing);  // 10% de 200 (faixa Level 1)
        Assert.Equal(40, highOutcome.Healing); // 20% de 200 (faixa Level 50)
        Assert.Equal(KnightHeal.ManaCost, lowOutcome.ManaCost);
        Assert.Equal(0, lowOutcome.Damage);
        Assert.Equal(0, random.Calls); // determinismo: a cura não consome aleatoriedade
    }

    [Fact]
    public void Resolver_HealingIsCappedAtMaximumHealth()
    {
        var player = Player(level: 100, maxHealth: 200, health: 190); // 25% = 50, faltam 10

        var outcome = Resolver().ResolveAction(new CombatTurnAction(player, player, CombatActionKind.UseHealingSpell), new CountingRandom());

        Assert.Equal(10, outcome.Healing);
    }

    [Fact]
    public void Resolver_WithoutMana_NoSpellHealing()
    {
        var player = Player(mana: KnightHeal.ManaCost - 1);
        var noSpell = new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Player, "N", 1, 200, 50, 100, 100, new CombatProfile(10, 14, 4, 6));

        Assert.False(Resolver().ResolveAction(new CombatTurnAction(player, player, CombatActionKind.UseHealingSpell), new CountingRandom()).Executed);
        Assert.False(Resolver().ResolveAction(new CombatTurnAction(noSpell, noSpell, CombatActionKind.UseHealingSpell), new CountingRandom()).Executed);
    }

    [Fact]
    public void Selector_SpellFirstWhenBelowLimitWithMana_PotionIsTheReserve()
    {
        var supplies = new CombatSupplyContext();
        supplies.Load(new SupplyPolicy(80, 30), healthPotions: 5, manaPotions: 5);
        var selector = Selector(supplies);

        // Abaixo de 50% e com Mana: spell (mesmo havendo poção de vida e política de poção).
        var hurt = Player(health: 90, mana: 100);
        Assert.Equal(CombatActionKind.UseHealingSpell, selector.SelectSecondaryChoice(Session(hurt), hurt)!.Kind);

        // Sem Mana para a spell: poção de vida (reserva).
        var dry = Player(health: 90, mana: KnightHeal.ManaCost - 1);
        Assert.Equal(CombatActionKind.UseHealthPotion, selector.SelectSecondaryChoice(Session(dry), dry)!.Kind);

        // Entre o limite da spell (50%) e o da poção (80%): poção, como antes.
        var scratched = Player(health: 140, mana: 100);
        Assert.Equal(CombatActionKind.UseHealthPotion, selector.SelectSecondaryChoice(Session(scratched), scratched)!.Kind);

        // Vida acima dos dois limites e Mana baixa: poção de Mana repõe a Mana.
        var healthy = Player(health: 200, mana: 10);
        Assert.Equal(CombatActionKind.UseManaPotion, selector.SelectSecondaryChoice(Session(healthy), healthy)!.Kind);

        // Política desligada: comportamento antigo (poção de vida).
        Assert.Equal(CombatActionKind.UseHealthPotion,
            Selector(supplies, HealingSpellPolicy.Disabled).SelectSecondaryChoice(Session(hurt), hurt)!.Kind);
    }

    [Fact]
    public void Selector_BelowLimitWithoutMana_DoesNotChooseTheSpell()
    {
        var supplies = new CombatSupplyContext();
        supplies.Load(SupplyPolicy.None, 0, 0);
        var dry = Player(health: 10, mana: 0);

        var choice = Selector(supplies).SelectSecondaryChoice(Session(dry), dry);

        Assert.True(choice is null || choice.Kind != CombatActionKind.UseHealingSpell);
    }

    private static CombatOrchestrator Orchestrator(CombatSupplyContext supplies) => new(
        Resolver(),
        new SequentialTurnScheduler(),
        Selector(supplies),
        []);

    private static (CombatRoundReport Report, Combatant Player) Run()
    {
        var supplies = new CombatSupplyContext();
        supplies.Load(SupplyPolicy.None, 0, 0);
        var player = Player(level: 1, maxHealth: 200, health: 40, mana: 100);
        return (Orchestrator(supplies).ExecuteRound(Session(player, seed: 7)), player);
    }

    [Fact]
    public void Orchestrator_SpellHealsTheCaster_SpendsMana_AndIsDeterministicWithTheSeed()
    {
        var (first, caster) = Run();
        var (second, _) = Run();

        var heal = Assert.Single(first.Log, e => e.Kind == CombatActionKind.UseHealingSpell);
        Assert.Equal(caster.Id, heal.ActorId);
        Assert.Equal(caster.Id, heal.TargetId);
        Assert.Equal(20, heal.Healing);
        Assert.Equal(100 - KnightHeal.ManaCost, caster.CurrentMana);
        Assert.Equal(
            first.Log.Select(e => (e.Kind, e.Damage, e.Healing, e.TargetHealthAfter)),
            second.Log.Select(e => (e.Kind, e.Damage, e.Healing, e.TargetHealthAfter)));
    }

    // Ressalva C da revisão do PV-COMBAT-014: vida cheia → NotExecuted, sem gastar Mana.
    [Fact]
    public void Resolver_AtFullHealth_DoesNotCastAndSpendsNoMana()
    {
        var full = Player(maxHealth: 200, health: 200, mana: 100);

        var outcome = Resolver().ResolveAction(new CombatTurnAction(full, full, CombatActionKind.UseHealingSpell), new CountingRandom());

        Assert.False(outcome.Executed);
        Assert.Equal(0, outcome.Healing);
        Assert.Equal(0, outcome.ManaCost);
        Assert.Equal(100, full.CurrentMana);
    }

    // Ressalva A da revisão do PV-COMBAT-014: o seletor RESOLVIDO PELA COMPOSIÇÃO REAL (DI) escolhe a
    // spell de cura com a vida abaixo do limite e Mana suficiente — prova que a política está ligada.
    [Fact]
    public void Composition_ResolvedSelector_ChoosesHealingSpell_WhenBelowLimitWithMana()
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var selector = scope.ServiceProvider.GetRequiredService<ICombatActionSelector>();
        Assert.IsType<SupplyAwareCombatActionSelector>(selector);

        var hurt = Player(maxHealth: 200, health: 90, mana: 100); // 45% < 50% (limite padrão)
        Assert.Equal(CombatActionKind.UseHealingSpell, selector.SelectSecondaryChoice(Session(hurt), hurt)!.Kind);

        // Acima do limite: não escolhe a spell.
        var fine = Player(maxHealth: 200, health: 150, mana: 100);
        Assert.NotEqual(CombatActionKind.UseHealingSpell, selector.SelectSecondaryChoice(Session(fine), fine)?.Kind);

        // Sem Mana: não escolhe a spell.
        var dry = Player(maxHealth: 200, health: 90, mana: KnightHeal.ManaCost - 1);
        Assert.NotEqual(CombatActionKind.UseHealingSpell, selector.SelectSecondaryChoice(Session(dry), dry)?.Kind);
    }

    // Ressalva A: o orquestrador resolvido pela composição cura o jogador ferido de ponta a ponta.
    [Fact]
    public void Composition_ResolvedOrchestrator_HealsHurtPlayer()
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var orchestrator = scope.ServiceProvider.GetRequiredService<CombatOrchestrator>();
        var player = Player(level: 1, maxHealth: 200, health: 40, mana: 100);

        var report = orchestrator.ExecuteRound(Session(player, seed: 7));

        var heal = Assert.Single(report.Log, e => e.Kind == CombatActionKind.UseHealingSpell);
        Assert.Equal(player.Id, heal.TargetId);
        Assert.True(heal.Healing > 0);
    }

    [Fact]
    public void Composition_RegistersTheDefaultPolicy()
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        using var provider = services.BuildServiceProvider();

        Assert.Equal(HealingSpellPolicy.DefaultBelowPercent, provider.GetRequiredService<HealingSpellPolicy>().BelowPercent);
    }
}
