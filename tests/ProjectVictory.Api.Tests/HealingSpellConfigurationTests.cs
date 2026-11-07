using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Domain.Combat.Rules;
using Xunit;

namespace ProjectVictory.Api.Tests;

// Ressalva B da revisão do PV-COMBAT-014: "Combat:HealingSpellBelowPercent" fora de 0..100 derruba o
// startup (mesmo padrão do ExperienceRateConfigurationTests). Ressalva A (pela API real): o host
// registra a política configurada e o seletor resolvido a usa.
[Collection("Api")]
public sealed class HealingSpellConfigurationTests
{
    private const string Variable = "Combat__HealingSpellBelowPercent";

    [Fact]
    public void DefaultConfiguration_IsFiftyPercent_AndTheResolvedSelectorUsesIt()
    {
        using var factory = new ApiFactory();

        Assert.Equal(HealingSpellPolicy.DefaultBelowPercent, factory.Services.GetRequiredService<HealingSpellPolicy>().BelowPercent);

        using var scope = factory.Services.CreateScope();
        Assert.IsType<ProjectVictory.Application.Supply.SupplyAwareCombatActionSelector>(
            scope.ServiceProvider.GetRequiredService<ICombatActionSelector>());
    }

    [Fact]
    public void ServerConfiguration_ChangesTheLimit()
    {
        var previous = Environment.GetEnvironmentVariable(Variable);
        Environment.SetEnvironmentVariable(Variable, "30");

        try
        {
            using var factory = new ApiFactory();

            Assert.Equal(30, factory.Services.GetRequiredService<HealingSpellPolicy>().BelowPercent);
        }
        finally
        {
            Environment.SetEnvironmentVariable(Variable, previous);
        }
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("101")]
    public void OutOfRange_FailsStartup(string value)
    {
        var previous = Environment.GetEnvironmentVariable(Variable);
        Environment.SetEnvironmentVariable(Variable, value);

        try
        {
            using var factory = new ApiFactory();

            var ex = Assert.ThrowsAny<Exception>(() => factory.Services.GetRequiredService<HealingSpellPolicy>());

            // A falha tem de vir da validação do limite da spell de cura.
            Assert.Contains(Flatten(ex), e =>
                e is InvalidOperationException && e.Message.Contains(HealingSpellPolicy.BelowPercentKey, StringComparison.Ordinal));
        }
        finally
        {
            Environment.SetEnvironmentVariable(Variable, previous);
        }
    }

    private static IEnumerable<Exception> Flatten(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            yield return current;
        }
    }
}
