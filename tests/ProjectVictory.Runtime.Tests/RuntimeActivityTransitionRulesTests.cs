using ProjectVictory.Domain.Runtime;
using Xunit;

namespace ProjectVictory.Runtime.Tests;

// PV-QA-001 parte 2: o projeto ProjectVictory.Runtime está vazio (só .csproj); as regras de
// runtime existentes vivem em ProjectVictory.Domain/Runtime. Estes testes documentam o
// comportamento atual da máquina de estados de atividade do Runtime.
public sealed class RuntimeActivityTransitionRulesTests
{
    [Theory]
    [InlineData(RuntimeActivity.Idle, RuntimeActivity.Exploring)]
    [InlineData(RuntimeActivity.Exploring, RuntimeActivity.Hunting)]
    [InlineData(RuntimeActivity.Exploring, RuntimeActivity.Traveling)]
    [InlineData(RuntimeActivity.Exploring, RuntimeActivity.Dialogue)]
    [InlineData(RuntimeActivity.Traveling, RuntimeActivity.Exploring)]
    [InlineData(RuntimeActivity.Hunting, RuntimeActivity.Combat)]
    [InlineData(RuntimeActivity.Hunting, RuntimeActivity.Exploring)]
    [InlineData(RuntimeActivity.Hunting, RuntimeActivity.Reward)]
    [InlineData(RuntimeActivity.Combat, RuntimeActivity.Reward)]
    [InlineData(RuntimeActivity.Reward, RuntimeActivity.Exploring)]
    [InlineData(RuntimeActivity.Dialogue, RuntimeActivity.Exploring)]
    public void AllowedTransitions(RuntimeActivity current, RuntimeActivity next)
    {
        Assert.True(RuntimeActivityTransitionRules.CanTransition(current, next));
    }

    [Theory]
    [InlineData(RuntimeActivity.Idle, RuntimeActivity.Hunting)]
    [InlineData(RuntimeActivity.Idle, RuntimeActivity.Combat)]
    [InlineData(RuntimeActivity.Combat, RuntimeActivity.Exploring)]
    [InlineData(RuntimeActivity.Reward, RuntimeActivity.Hunting)]
    [InlineData(RuntimeActivity.Traveling, RuntimeActivity.Hunting)]
    public void ForbiddenTransitions(RuntimeActivity current, RuntimeActivity next)
    {
        Assert.False(RuntimeActivityTransitionRules.CanTransition(current, next));
    }

    [Fact]
    public void SameActivity_IsAllowed()
    {
        foreach (var activity in Enum.GetValues<RuntimeActivity>())
        {
            Assert.True(RuntimeActivityTransitionRules.CanTransition(activity, activity));
        }
    }
}
