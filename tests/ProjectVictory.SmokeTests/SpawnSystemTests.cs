using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Spawn;
using ProjectVictory.Application.Supply;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.Spawn;
using ProjectVictory.Infrastructure.Content;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-SPAWN-010 (ADR-009/010/047): Spawn System stateless, dirigido pela configuração da Hunt,
// com o mesmo comportamento externo do gerador de encounters anterior.
public sealed class SpawnSystemTests
{
    private static readonly HuntAreaDefinition SpiderCave = new SpiderCaveHuntArea().GetDefault();

    // O sorteio que ficava no EncounterGenerator (antes do PV-SPAWN-010).
    private static IReadOnlyList<Guid> OldPick(int seed) =>
        SpiderCaveCatalog.EncounterTemplates[new SeededRandomSource(seed).NextInclusive(0, SpiderCaveCatalog.EncounterTemplates.Count - 1)];

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(7)]
    [InlineData(42)]
    [InlineData(555)]
    public void SameSeed_SamePack_AsTheOldGenerator(int seed)
    {
        var result = new StatelessSpawnSystem().SpawnPack(new SpawnContext(Guid.NewGuid(), SpiderCave, seed));

        Assert.Equal(OldPick(seed), result.Monsters.Select(m => m.MonsterId));
        Assert.Equal(result.Monsters.Select(m => m.MonsterId), result.Instances.Select(i => i.SpawnDefinitionId));
        Assert.All(result.Monsters, m => Assert.Equal(SpiderCaveCatalog.Find(m.MonsterId)!.MaximumHealth, m.MaximumHealth));
    }

    [Fact]
    public void Pack_IsActive_ThenDespawned_AndTheSystemKeepsNoState()
    {
        var spawn = new StatelessSpawnSystem();
        var context = new SpawnContext(Guid.NewGuid(), SpiderCave, 3);

        var first = spawn.SpawnPack(context);
        var second = spawn.SpawnPack(context);

        Assert.All(first.Instances, i => Assert.Equal(SpawnState.Active, i.CurrentState));
        Assert.All(first.Instances, i => Assert.NotNull(i.SpawnedAt));

        // Sem estado: cada pack é criado sob demanda, com instâncias novas (mesma composição).
        Assert.Equal(first.Monsters.Select(m => m.MonsterId), second.Monsters.Select(m => m.MonsterId));
        Assert.Empty(first.Instances.Select(i => i.SpawnInstanceId).Intersect(second.Instances.Select(i => i.SpawnInstanceId)));

        spawn.Despawn(first.Instances);
        Assert.All(first.Instances, i => Assert.Equal(SpawnState.Despawned, i.CurrentState));
        Assert.All(second.Instances, i => Assert.Equal(SpawnState.Active, i.CurrentState));
    }

    [Fact]
    public void InvalidContext_IsRejected()
    {
        var spawn = new StatelessSpawnSystem();

        Assert.Throws<ArgumentException>(() => spawn.SpawnPack(new SpawnContext(Guid.Empty, SpiderCave, 1)));
        Assert.Throws<InvalidOperationException>(() => spawn.SpawnPack(new SpawnContext(Guid.NewGuid(), new HuntAreaDefinition("Empty", []), 1)));
    }

    [Fact]
    public void Composition_EncounterGenerator_UsesTheSpawnSystem()
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        Assert.IsType<StatelessSpawnSystem>(provider.GetRequiredService<ISpawnSystem>());

        var encounter = scope.ServiceProvider.GetRequiredService<EncounterGenerator>().Generate(Guid.NewGuid(), 42);
        Assert.Equal(OldPick(42), encounter.Monsters.Select(m => m.MonsterId));
        Assert.Equal(42, encounter.Seed);
    }

    // Revisão do PV-XP-010 (ressalva 4): poção "usada" por um monstro não sai do inventário do jogador.
    [Fact]
    public void SupplyObserver_IgnoresMonsterPotionActions()
    {
        var supplies = new CombatSupplyContext();
        supplies.Load(SupplyPolicy.None, 2, 2);
        var observer = new SupplyConsumptionObserver(supplies);

        var player = new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Player, "P", 1, 100, 100, 50, 50, new CombatProfile(10, 10, 0, 0));
        var monster = new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Monster, "M", 1, 100, 100, 0, 0, new CombatProfile(5, 5, 0, 0));
        var session = new CombatSession(Guid.NewGuid(), player, [monster], new SeededRandomSource(1));
        var outcome = new CombatActionOutcome(0, 10, 0, 0, true);

        observer.OnActionExecuted(session, new CombatTurnAction(monster, monster, CombatActionKind.UseHealthPotion), outcome);
        Assert.Equal(0, supplies.HealthPotionsUsed);

        observer.OnActionExecuted(session, new CombatTurnAction(player, player, CombatActionKind.UseHealthPotion), outcome);
        Assert.Equal(1, supplies.HealthPotionsUsed);
    }
}
