using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;
using Xunit;

namespace ProjectVictory.Application.Tests;

// PV-MONSTER-010 (ADR-005/006): Monster AI única, orientada a dados, determinística com a semente.
public sealed class MonsterAITests
{
    private static readonly Guid BiterId = Guid.NewGuid();

    private static readonly SkillDefinition Bite =
        new("monster-bite", "Bite", CharacterClass.Knight, SkillTargeting.SingleTarget, 0, 2m, 1, SkillScaling.Weapon);

    private sealed class Behaviors(Dictionary<Guid, MonsterBehavior> data) : IMonsterBehaviorProvider
    {
        public MonsterBehavior Get(Guid monsterId) =>
            data.TryGetValue(monsterId, out var behavior) ? behavior : MonsterBehavior.BasicAttackOnly;
    }

    private sealed class CountingRandom(IRandomSource inner) : IRandomSource
    {
        public int Calls { get; private set; }

        public double NextDouble()
        {
            Calls++;
            return inner.NextDouble();
        }

        public int NextInclusive(int minInclusive, int maxInclusive)
        {
            Calls++;
            return inner.NextInclusive(minInclusive, maxInclusive);
        }
    }

    private static CombatSession Session(IRandomSource random, Guid monsterSource)
    {
        var player = new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Player, "P", 5, 500, 500, 0, 0, new CombatProfile(10, 10, 0, 0));
        var monster = new Combatant(Guid.NewGuid(), monsterSource, CombatSide.Monster, "M", 1, 100, 100, 0, 0, new CombatProfile(5, 5, 0, 0));

        return new CombatSession(Guid.NewGuid(), player, [monster], random);
    }

    [Fact]
    public void NoRules_BasicAttackOnly_ConsumesNoRandomness()
    {
        var random = new CountingRandom(new SeededRandomSource(1));
        var session = Session(random, Guid.NewGuid());
        var ai = new DataDrivenMonsterAI(BasicAttackOnlyBehaviorProvider.Instance);

        Assert.Null(ai.SelectSecondaryChoice(session, session.Monsters[0]));
        Assert.Equal(0, random.Calls);
    }

    [Fact]
    public void Rule_ComesFromData_AndIsDeterministicWithTheSeed()
    {
        var behaviors = new Behaviors(new() { [BiterId] = new MonsterBehavior([new MonsterActionRule(CombatActionKind.UseSkill, 50, Bite)]) });
        var ai = new DataDrivenMonsterAI(behaviors);

        static List<bool> Run(DataDrivenMonsterAI ai, int seed)
        {
            var session = Session(new SeededRandomSource(seed), BiterId);
            return Enumerable.Range(0, 40).Select(_ => ai.SelectSecondaryChoice(session, session.Monsters[0]) is not null).ToList();
        }

        var first = Run(ai, 7);
        Assert.Equal(first, Run(ai, 7));            // mesma semente → mesmas escolhas
        Assert.Contains(true, first);              // 50%: usa às vezes…
        Assert.Contains(false, first);             // …e às vezes não

        var always = new DataDrivenMonsterAI(new Behaviors(new() { [BiterId] = new MonsterBehavior([new MonsterActionRule(CombatActionKind.UseSkill, 100, Bite)]) }));
        var session = Session(new SeededRandomSource(1), BiterId);
        var choice = always.SelectSecondaryChoice(session, session.Monsters[0]);
        Assert.Equal(CombatActionKind.UseSkill, choice!.Kind);
        Assert.Equal("Bite", choice.Skill!.Name);

        // A IA não escolhe pelo jogador.
        Assert.Null(always.SelectSecondaryChoice(session, session.Player));
    }

    private sealed class FixedDamageResolver : ICombatResolver
    {
        public CombatResult Resolve(CombatRequest request) => throw new NotSupportedException();

        public CombatActionOutcome ResolveAction(CombatTurnAction action, IRandomSource random) => action.Kind switch
        {
            CombatActionKind.BasicAttack => new CombatActionOutcome(1, 0, 0, 0, true),
            CombatActionKind.UseSkill => new CombatActionOutcome(9, 0, 0, 0, true),
            _ => CombatActionOutcome.NotExecuted
        };
    }

    [Fact]
    public void Orchestrator_MonsterUsesDataDrivenAbility_ThroughTheSelector()
    {
        var behaviors = new Behaviors(new() { [BiterId] = new MonsterBehavior([new MonsterActionRule(CombatActionKind.UseSkill, 100, Bite)]) });
        var selector = new DefaultCombatActionSelector(SkillSelectionPolicy.Default, new DataDrivenMonsterAI(behaviors));
        var orchestrator = new CombatOrchestrator(new FixedDamageResolver(), new SequentialTurnScheduler(), selector, []);
        var session = Session(new SeededRandomSource(3), BiterId);

        var report = orchestrator.ExecuteRound(session);

        var monsterSkill = Assert.Single(report.Log, e => e.Kind == CombatActionKind.UseSkill);
        Assert.Equal("Bite", monsterSkill.SkillName);
        Assert.Equal(500 - 1 - 9, session.Player.CurrentHealth); // ataque básico (1) + Bite (9)
    }

    [Fact]
    public void Rules_AreValidated()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MonsterActionRule(CombatActionKind.UseSkill, 0, Bite));
        Assert.Throws<ArgumentOutOfRangeException>(() => new MonsterActionRule(CombatActionKind.UseSkill, 101, Bite));
        Assert.Throws<ArgumentException>(() => new MonsterActionRule(CombatActionKind.UseSkill, 50));
        Assert.Throws<ArgumentException>(() => new MonsterActionRule(CombatActionKind.BasicAttack, 50));
    }
}
