using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Domain.Combat.Rules;
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
}
