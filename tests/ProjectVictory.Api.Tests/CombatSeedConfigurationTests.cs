using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Combat.TurnBased;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-COMBAT-012 (revisão do PV-COMBAT-011, ressalva 2): Combat:FixedSeed só vale em Development.
[Collection("Api")]
public sealed class CombatSeedConfigurationTests
{
    private const string SeedVariable = "Combat__FixedSeed";
    private const string MigrateVariable = "Database__ApplyMigrationsOnStartup";

    [Fact]
    public void FixedSeed_IsHonoredInDevelopment()
    {
        var previous = Environment.GetEnvironmentVariable(SeedVariable);
        Environment.SetEnvironmentVariable(SeedVariable, "5");

        try
        {
            using var factory = new ApiFactory();

            Assert.Equal(5, factory.Services.GetRequiredService<CombatSettings>().FixedSeed);
        }
        finally
        {
            Environment.SetEnvironmentVariable(SeedVariable, previous);
        }
    }

    [Fact]
    public void FixedSeed_IsIgnoredOutsideDevelopment()
    {
        var previousSeed = Environment.GetEnvironmentVariable(SeedVariable);
        var previousMigrate = Environment.GetEnvironmentVariable(MigrateVariable);
        Environment.SetEnvironmentVariable(SeedVariable, "5");
        Environment.SetEnvironmentVariable(MigrateVariable, "true");

        try
        {
            using var factory = new ApiFactory("Production");

            Assert.Null(factory.Services.GetRequiredService<CombatSettings>().FixedSeed);
        }
        finally
        {
            Environment.SetEnvironmentVariable(SeedVariable, previousSeed);
            Environment.SetEnvironmentVariable(MigrateVariable, previousMigrate);
        }
    }
}
