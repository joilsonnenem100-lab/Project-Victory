using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Infrastructure.Combat;
using ProjectVictory.Infrastructure.Services;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-COMBAT-013 (revisão, B1 e ressalva 3): skills com o CombatResolver REAL. Movidos do
// Application.Tests para cá, que já referencia Infrastructure.
public sealed class ClassSkillCombatTests
{
    private static CombatSession Session(int playerMana, int monsters, int seed = 1)
    {
        var player = new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Player, "P", 20, 500, 500, 100, playerMana,
            new CombatProfile(10, 14, 4, 6), ClassSkillTable.UnlockedFor(CharacterClass.Knight, 20));
        var enemies = Enumerable.Range(0, monsters)
            .Select(i => new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Monster, $"M{i}", 1, 1000, 1000, 0, 0, new CombatProfile(5, 5, 0, 0)))
            .ToList();

        return new CombatSession(Guid.NewGuid(), player, enemies, new SeededRandomSource(seed));
    }

    private static CombatOrchestrator Orchestrator() => new(
        new CombatResolver(new ProvisionalTibiaLikeDamageFormula(), new ProvisionalSupplyEffects()),
        new SequentialTurnScheduler(),
        new DefaultCombatActionSelector(),
        []);

    // B1: com mana exatamente igual ao custo, a skill em área atinge os 3 e a mana zera.
    [Fact]
    public void AreaSkill_RealResolver_ManaEqualToCost_HitsAllTargets()
    {
        var groundShaker = ClassSkillTable.ForClass(CharacterClass.Knight).Single(s => s.Name == "Ground Shaker");
        var session = Session(playerMana: groundShaker.ManaCost, monsters: 3);

        var report = Orchestrator().ExecuteRound(session);

        var skillTurns = report.Log.Where(e => e.Kind == CombatActionKind.UseSkill).ToList();
        Assert.Equal(3, skillTurns.Count);
        Assert.All(skillTurns, e => Assert.Equal("Ground Shaker", e.SkillName));
        Assert.Equal(3, skillTurns.Select(e => e.TargetId).Distinct().Count());
        Assert.Equal(0, session.Player.CurrentMana);
    }

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

    // Revisão final do PV-COMBAT-013 (ressalva 1): o guard de mana do resolver — sem mana, a skill
    // em área não executa e NÃO consome número aleatório (determinismo).
    [Fact]
    public void ResolveSkill_NotEnoughMana_NotExecuted_NoRandomConsumed()
    {
        var resolver = new CombatResolver(new ProvisionalTibiaLikeDamageFormula(), new ProvisionalSupplyEffects());
        var groundShaker = ClassSkillTable.ForClass(CharacterClass.Knight).Single(s => s.Name == "Ground Shaker");
        var session = Session(playerMana: groundShaker.ManaCost - 1, monsters: 3);
        var random = new CountingRandom();

        var result = resolver.ResolveSkill(session.Player, groundShaker, session.Monsters, random);

        Assert.False(result.Executed);
        Assert.Empty(result.Hits);
        Assert.Equal(0, random.Calls);

        // Com mana suficiente: um sorteio de dano por alvo, custo único.
        var paid = Session(playerMana: groundShaker.ManaCost, monsters: 3);
        var ok = resolver.ResolveSkill(paid.Player, groundShaker, paid.Monsters, new CountingRandom());
        Assert.True(ok.Executed);
        Assert.Equal(3, ok.Hits.Count);
        Assert.Equal(groundShaker.ManaCost, ok.ManaCost);
        Assert.All(ok.Hits, h => Assert.Equal(0, h.Outcome.ManaCost));
    }

    // Com pouca mana o seletor nem escolhe skill (o round segue só com o ataque básico).
    [Fact]
    public void AreaSkill_RealResolver_NotEnoughMana_SelectorChoosesNoSkill()
    {
        var session = Session(playerMana: 5, monsters: 3);

        var report = Orchestrator().ExecuteRound(session);

        Assert.DoesNotContain(report.Log, e => e.Kind == CombatActionKind.UseSkill);
        Assert.Equal(5, session.Player.CurrentMana);
    }

    [Fact]
    public void RealFormula_SameSeed_SameSkillCombat()
    {
        static List<(string?, int)> Run(int seed)
        {
            var session = Session(playerMana: 100, monsters: 3, seed);
            var orchestrator = Orchestrator();
            var log = new List<(string?, int)>();

            for (var i = 0; i < 50 && !session.IsFinished; i++)
            {
                log.AddRange(orchestrator.ExecuteRound(session).Log.Select(e => (e.SkillName, e.Damage)));
            }

            return log;
        }

        Assert.Equal(Run(9), Run(9));
        Assert.Contains(Run(9), e => e.Item1 is not null);
    }
}
