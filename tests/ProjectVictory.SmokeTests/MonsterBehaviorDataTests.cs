using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Infrastructure.Combat;
using ProjectVictory.Infrastructure.Content;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-MONSTER-010: o comportamento atual dos monstros da Spider Cave foi migrado para dados (só o
// ataque básico) e a composição usa a Monster AI orientada a dados.
public sealed class MonsterBehaviorDataTests
{
    [Fact]
    public void SpiderCaveMonsters_DeclareBehaviorInData_BasicAttackOnly()
    {
        var provider = new MonsterBehaviorProvider();

        Assert.All(SpiderCaveCatalog.Monsters, monster =>
        {
            Assert.NotNull(monster.Behavior);
            Assert.Empty(provider.Get(monster.Id).Rules);
        });

        Assert.Same(MonsterBehavior.BasicAttackOnly, provider.Get(Guid.NewGuid()));
    }

    [Fact]
    public void Composition_RegistersTheSingleDataDrivenMonsterAI()
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        using var provider = services.BuildServiceProvider();

        Assert.IsType<DataDrivenMonsterAI>(provider.GetRequiredService<IMonsterAI>());
        Assert.IsType<MonsterBehaviorProvider>(provider.GetRequiredService<IMonsterBehaviorProvider>());
    }

    private sealed class FixedMonsterAI : IMonsterAI
    {
        public CombatTurnChoice? SelectSecondaryChoice(CombatSession session, Combatant monster) =>
            new(CombatActionKind.UseSkill, null);
    }

    // Revisão do PV-MONSTER-010 (ressalva 2): a DI escolhe o construtor do seletor COM a Monster AI.
    [Fact]
    public void Selector_ResolvedByDI_UsesTheRegisteredMonsterAI()
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        services.AddSingleton<IMonsterAI, FixedMonsterAI>();
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var selector = scope.ServiceProvider.GetRequiredService<ICombatActionSelector>();
        var player = new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Player, "P", 1, 100, 100, 0, 0, new CombatProfile(10, 10, 0, 0));
        var monster = new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Monster, "M", 1, 100, 100, 0, 0, new CombatProfile(5, 5, 0, 0));
        var session = new CombatSession(Guid.NewGuid(), player, [monster], new SeededRandomSource(1));

        Assert.Equal(CombatActionKind.UseSkill, selector.SelectSecondaryChoice(session, monster)!.Kind);
    }
}
