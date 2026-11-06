using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;
using Xunit;

namespace ProjectVictory.Application.Tests;

// PV-COMBAT-011: combate por turnos determinístico (ADR-052): ordem fixa, um ativo por turno e
// aleatoriedade só pelo gerador com semente.
public sealed class TurnDeterminismTests
{
    // Resolver que usa o gerador da sessão: dano 1..12 sorteado.
    private sealed class RandomDamageResolver : ICombatResolver
    {
        public CombatResult Resolve(CombatRequest request) => throw new NotSupportedException();

        public CombatActionOutcome ResolveAction(CombatTurnAction action, IRandomSource random) =>
            action.Kind == CombatActionKind.BasicAttack
                ? new CombatActionOutcome(random.NextInclusive(1, 12), 0, 0, 0, true)
                : CombatActionOutcome.NotExecuted;
    }

    private static CombatSession NewSession(int seed) =>
        new(
            Guid.NewGuid(),
            new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Player, "Knight", 1, 185, 185, 55, 55, new CombatProfile(10, 14, 8, 10)),
            [
                new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Monster, "Rat", 1, 20, 20, 0, 0, new CombatProfile(8, 6, 1, 1)),
                new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Monster, "Spider", 1, 30, 30, 0, 0, new CombatProfile(10, 8, 2, 2))
            ],
            new SeededRandomSource(seed));

    private static List<(string Actor, int Damage, int TargetHp)> Play(int seed)
    {
        var session = NewSession(seed);
        var orchestrator = new CombatOrchestrator(
            new RandomDamageResolver(), new SequentialTurnScheduler(), new NoSecondaryActionSelector(), []);
        var log = new List<(string, int, int)>();

        while (!session.IsFinished)
        {
            log.AddRange(orchestrator.ExecuteRound(session).Log.Select(e => (e.ActorName, e.Damage, e.TargetHealthAfter)));
        }

        return log;
    }

    [Fact]
    public void SameSeed_ProducesIdenticalCombat()
    {
        Assert.Equal(Play(1234), Play(1234));
    }

    [Fact]
    public void DifferentSeeds_ProduceDifferentCombats()
    {
        Assert.NotEqual(Play(1), Play(2));
    }

    [Fact]
    public void SeededRandomSource_IsReproducible()
    {
        var a = new SeededRandomSource(42);
        var b = new SeededRandomSource(42);

        for (var i = 0; i < 50; i++)
        {
            Assert.Equal(a.NextInclusive(0, 1000), b.NextInclusive(0, 1000));
            Assert.Equal(a.NextDouble(), b.NextDouble());
        }

        Assert.Equal(42, a.Seed);
        Assert.Throws<ArgumentException>(() => a.NextInclusive(5, 4));
    }

    [Fact]
    public void Scheduler_ExactlyOneActive_SkipsDead_AndWrapsToPlayer()
    {
        var session = NewSession(7);
        var scheduler = new SequentialTurnScheduler();
        var rat = session.Monsters[0];
        var spider = session.Monsters[1];

        Assert.Same(session.Player, session.ActiveParticipant);
        Assert.Same(rat, scheduler.Next(session));

        session.AdvanceTo(rat);
        Assert.Same(spider, scheduler.Next(session));

        rat.ApplyDamage(100);
        session.AdvanceTo(session.Player);
        Assert.Same(spider, scheduler.Next(session)); // Rat morto é pulado

        session.AdvanceTo(spider);
        Assert.Same(session.Player, scheduler.Next(session));
    }

    [Fact]
    public void SeedProvider_UsesFixedSeedOnlyWhenConfigured()
    {
        Assert.Equal(99, new CombatSeedProvider(new CombatSettings(99)).NextSeed());
        Assert.Equal(99, new CombatSeedProvider(new CombatSettings(99)).NextSeed());

        var random = new CombatSeedProvider(new CombatSettings());
        var seeds = Enumerable.Range(0, 20).Select(_ => random.NextSeed()).Distinct().Count();
        Assert.True(seeds > 1);
    }

    [Fact]
    public void Encounter_KeepsItsSeed()
    {
        var encounter = new EncounterGenerator().Generate(Guid.NewGuid(), 555);

        Assert.Equal(555, encounter.Seed);
    }
}
