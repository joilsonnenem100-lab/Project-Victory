using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Infrastructure.Combat;
using ProjectVictory.Infrastructure.Services;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-COMBAT-012: resolver real (substitui o stub) + caminhos do orquestrador pedidos na revisão
// do PV-COMBAT-010.
public sealed class CombatResolverTests
{
    private static CombatResolver Resolver() =>
        new(new ProvisionalTibiaLikeDamageFormula(), new ProvisionalSupplyEffects());

    private static Combatant Player(int mana, int abilityCost = 10) =>
        new(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Player, "Knight", 1, 185, 100, 55, mana,
            new CombatProfile(10, 14, 8, 10, abilityCost, 1.5m));

    private static Combatant Rat() =>
        new(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Monster, "Rat", 1, 20, 20, 0, 0, new CombatProfile(8, 6, 1, 1));

    [Fact]
    public void BasicAttack_UsesTheFormula_WithinMaximum()
    {
        var random = new SeededRandomSource(3);

        for (var i = 0; i < 50; i++)
        {
            var outcome = Resolver().ResolveAction(new CombatTurnAction(Player(55), Rat(), CombatActionKind.BasicAttack), random);

            Assert.True(outcome.Executed);
            Assert.InRange(outcome.Damage, 0, 12);
        }
    }

    [Fact]
    public void ClassAbility_WithoutMana_IsNotExecuted_AndConsumesNoRandom()
    {
        var random = new SeededRandomSource(9);
        var reference = new SeededRandomSource(9);

        var outcome = Resolver().ResolveAction(new CombatTurnAction(Player(5), Rat(), CombatActionKind.ClassAbility), random);

        Assert.False(outcome.Executed);
        Assert.Equal(reference.NextInclusive(0, 1000), random.NextInclusive(0, 1000));
    }

    [Fact]
    public void ClassAbility_WithMana_CostsMana()
    {
        var outcome = Resolver().ResolveAction(new CombatTurnAction(Player(55), Rat(), CombatActionKind.ClassAbility), new SeededRandomSource(1));

        Assert.True(outcome.Executed);
        Assert.Equal(10, outcome.ManaCost);
    }

    [Fact]
    public void Potions_UseProvisionalSupplyEffects()
    {
        var player = Player(55);

        Assert.Equal(75, Resolver().ResolveAction(new CombatTurnAction(player, player, CombatActionKind.UseHealthPotion), new SeededRandomSource(1)).Healing);
        Assert.Equal(50, Resolver().ResolveAction(new CombatTurnAction(player, player, CombatActionKind.UseManaPotion), new SeededRandomSource(1)).ManaRestored);
    }

    [Fact]
    public void LegacyResolve_StillReturnsAttackerAsWinner()
    {
        var attacker = Guid.NewGuid();

        Assert.Equal(attacker, Resolver().Resolve(new CombatRequest(Guid.NewGuid(), attacker, Guid.NewGuid())).WinnerId);
    }

    // --- caminhos do orquestrador (revisão do PV-COMBAT-010) ---

    private sealed class Recorder : ICombatActionObserver
    {
        public List<CombatActionKind> Kinds { get; } = [];

        public void OnActionExecuted(CombatSession session, CombatTurnAction action, CombatActionOutcome outcome) =>
            Kinds.Add(action.Kind);
    }

    private sealed class NeverPlayerScheduler : ITurnScheduler
    {
        public Combatant Next(CombatSession session) => session.Monsters[0];
    }

    private sealed class ZeroDamageResolver : ICombatResolver
    {
        public CombatResult Resolve(CombatRequest request) => throw new NotSupportedException();

        public CombatActionOutcome ResolveAction(CombatTurnAction action, IRandomSource random) =>
            action.Kind == CombatActionKind.BasicAttack ? new CombatActionOutcome(0, 0, 0, 0, true) : CombatActionOutcome.NotExecuted;
    }

    [Fact]
    public void Orchestrator_SignalsTurnLimit()
    {
        var session = new CombatSession(Guid.NewGuid(), Player(55), [Rat()], new SeededRandomSource(1));
        var orchestrator = new CombatOrchestrator(new ZeroDamageResolver(), new NeverPlayerScheduler(), new NoSecondaryActionSelector(), []);

        var report = orchestrator.ExecuteRound(session);

        Assert.True(report.TurnLimitReached);
        Assert.Equal(CombatSessionOutcome.Ongoing, report.Outcome);
        Assert.Equal(CombatOrchestrator.MaximumTurnsPerRound, report.Log.Count);
    }

    [Fact]
    public void Orchestrator_NotifiesObservers_AndSkipsNotExecutedSecondary()
    {
        var recorder = new Recorder();
        var player = Player(0); // sem mana: a ClassAbility não executa
        var session = new CombatSession(Guid.NewGuid(), player, [Rat()], new SeededRandomSource(5));
        var orchestrator = new CombatOrchestrator(Resolver(), new SequentialTurnScheduler(), new AlwaysAbility(), [recorder]);

        var report = orchestrator.ExecuteRound(session);

        Assert.False(report.TurnLimitReached);
        Assert.DoesNotContain(CombatActionKind.ClassAbility, recorder.Kinds);
        Assert.Equal(report.Log.Count, recorder.Kinds.Count);
    }

    private sealed class AlwaysAbility : ICombatActionSelector
    {
        public CombatActionKind? SelectSecondaryAction(CombatSession session, Combatant actor) =>
            actor.Side == CombatSide.Player ? CombatActionKind.ClassAbility : null;
    }

    [Fact]
    public void MonsterProfiles_HaveDataAndFallback()
    {
        var provider = new MonsterCombatProfileProvider();

        Assert.Equal(new CombatProfile(10, 8, 1, 1), provider.Get(Guid.Parse("10000000-0000-0000-0000-000000000001")).Profile);
        Assert.Same(MonsterCombatProfileProvider.Fallback, provider.Get(Guid.NewGuid()));
    }
}
