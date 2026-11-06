using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;
using Xunit;

namespace ProjectVictory.Application.Tests;

// PV-COMBAT-010: o Combat Orchestrator coordena turnos e etapas; as regras vêm de fora.
public sealed class CombatOrchestratorTests
{
    private sealed class ZeroRandom : IRandomSource
    {
        public double NextDouble() => 0;

        public int NextInclusive(int minInclusive, int maxInclusive) => minInclusive;
    }

    // Resolver falso: dano fixo por lado, registra as ações recebidas.
    private sealed class FakeResolver(int playerDamage, int monsterDamage) : ICombatResolver
    {
        public List<CombatTurnAction> Actions { get; } = [];

        public CombatResult Resolve(CombatRequest request) => throw new NotSupportedException();

        public CombatActionOutcome ResolveAction(CombatTurnAction action, IRandomSource random)
        {
            Actions.Add(action);

            return action.Kind switch
            {
                CombatActionKind.BasicAttack => new CombatActionOutcome(
                    action.Actor.Side == CombatSide.Player ? playerDamage : monsterDamage, 0, 0, 0, true),
                CombatActionKind.UseHealthPotion => new CombatActionOutcome(0, 30, 0, 0, true),
                _ => CombatActionOutcome.NotExecuted
            };
        }
    }

    private sealed class FixedSelector(CombatActionKind? playerChoice) : ICombatActionSelector
    {
        public int Calls { get; private set; }

        public CombatActionKind? SelectSecondaryAction(CombatSession session, Combatant actor)
        {
            Calls++;
            return actor.Side == CombatSide.Player ? playerChoice : null;
        }
    }

    private static Combatant Player(int hp = 100) =>
        new(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Player, "Knight", 1, 100, hp, 50, 50, new CombatProfile(10, 10, 0, 0));

    private static Combatant Monster(string name, int hp) =>
        new(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Monster, name, 1, hp, hp, 0, 0, new CombatProfile(10, 10, 0, 0));

    private static CombatOrchestrator Orchestrator(ICombatResolver resolver, ICombatActionSelector? selector = null) =>
        new(resolver, new SequentialTurnScheduler(), selector ?? new NoSecondaryActionSelector(), []);

    [Fact]
    public void Round_RunsPlayerThenEachLivingMonster_OneActivePerTurn_UntilPlayerTurnAgain()
    {
        var resolver = new FakeResolver(playerDamage: 5, monsterDamage: 3);
        var session = new CombatSession(Guid.NewGuid(), Player(), [Monster("Rat", 50), Monster("Cave Rat", 50)], new ZeroRandom());

        var report = Orchestrator(resolver).ExecuteRound(session);

        Assert.Equal(new[] { "Knight", "Rat", "Cave Rat" }, report.Log.Select(e => e.ActorName));
        Assert.Equal(new[] { 1, 2, 3 }, report.Log.Select(e => e.Turn));
        Assert.Equal(CombatSide.Player, session.ActiveParticipant.Side);
        Assert.Equal(4, session.TurnNumber);
        Assert.Equal(CombatSessionOutcome.Ongoing, report.Outcome);
    }

    [Fact]
    public void Damage_ComesOnlyFromTheResolver()
    {
        var resolver = new FakeResolver(playerDamage: 7, monsterDamage: 11);
        var player = Player();
        var rat = Monster("Rat", 50);
        var session = new CombatSession(Guid.NewGuid(), player, [rat], new ZeroRandom());

        Orchestrator(resolver).ExecuteRound(session);

        Assert.Equal(43, rat.CurrentHealth);
        Assert.Equal(89, player.CurrentHealth);
        Assert.All(resolver.Actions, a => Assert.Equal(CombatActionKind.BasicAttack, a.Kind));
    }

    [Fact]
    public void Victory_WhenAllMonstersDie_DeadMonstersAreSkipped()
    {
        var resolver = new FakeResolver(playerDamage: 50, monsterDamage: 1);
        var session = new CombatSession(Guid.NewGuid(), Player(), [Monster("Rat", 50), Monster("Spider", 50)], new ZeroRandom());
        var orchestrator = Orchestrator(resolver);

        var first = orchestrator.ExecuteRound(session);
        var second = orchestrator.ExecuteRound(session);

        // Rodada 1: jogador mata o Rat; só a Spider responde. Rodada 2: jogador mata a Spider.
        Assert.Equal(new[] { "Knight", "Spider" }, first.Log.Select(e => e.ActorName));
        Assert.Equal(CombatSessionOutcome.Victory, second.Outcome);
        Assert.True(session.IsFinished);
    }

    [Fact]
    public void Defeat_WhenPlayerDies_StopsImmediately()
    {
        var resolver = new FakeResolver(playerDamage: 1, monsterDamage: 60);
        var session = new CombatSession(Guid.NewGuid(), Player(100), [Monster("Giant Spider", 500), Monster("Spider", 500)], new ZeroRandom());

        var report = Orchestrator(resolver).ExecuteRound(session);

        Assert.Equal(CombatSessionOutcome.Defeat, report.Outcome);
        Assert.Equal(0, session.Player.CurrentHealth);
        Assert.Equal(3, report.Log.Count);
    }

    [Fact]
    public void SecondaryAction_IsChosenOutside_AndAppliedAfterBasicAttack()
    {
        var resolver = new FakeResolver(playerDamage: 5, monsterDamage: 40);
        var selector = new FixedSelector(CombatActionKind.UseHealthPotion);
        var player = Player(50);
        var session = new CombatSession(Guid.NewGuid(), player, [Monster("Rat", 100)], new ZeroRandom());

        var report = Orchestrator(resolver, selector).ExecuteRound(session);

        Assert.Equal(new[] { CombatActionKind.BasicAttack, CombatActionKind.UseHealthPotion, CombatActionKind.BasicAttack },
            report.Log.Select(e => e.Kind));
        Assert.Equal(40, player.CurrentHealth); // 50 + 30 (cura) − 40 (Rat)
        Assert.Equal(2, selector.Calls);
    }
}
