using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Reward;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-REWARD-010 / DEC-20261106-12: XP rate é configuração do SERVIDOR, padrão 1x,
// e nenhum endpoint o expõe no M2.
[Collection("Api")]
public sealed class ExperienceRateConfigurationTests
{
    private const string RateVariable = "Progression__ExperienceRate";

    [Fact]
    public void DefaultConfiguration_IsOneX()
    {
        using var factory = new ApiFactory();

        var settings = factory.Services.GetRequiredService<RewardSettings>();

        Assert.Equal(1m, settings.ExperienceRate.Value);
    }

    [Fact]
    public void ServerConfiguration_ChangesRate()
    {
        var previous = Environment.GetEnvironmentVariable(RateVariable);
        Environment.SetEnvironmentVariable(RateVariable, "2");

        try
        {
            using var factory = new ApiFactory();

            Assert.Equal(2m, factory.Services.GetRequiredService<RewardSettings>().ExperienceRate.Value);
        }
        finally
        {
            Environment.SetEnvironmentVariable(RateVariable, previous);
        }
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    public void InvalidRate_FailsStartup(string value)
    {
        var previous = Environment.GetEnvironmentVariable(RateVariable);
        Environment.SetEnvironmentVariable(RateVariable, value);

        try
        {
            using var factory = new ApiFactory();

            Assert.ThrowsAny<Exception>(() => factory.Services.GetRequiredService<RewardSettings>());
        }
        finally
        {
            Environment.SetEnvironmentVariable(RateVariable, previous);
        }
    }

    [Fact]
    public void NoEndpoint_ExposesExperienceRate()
    {
        using var factory = new ApiFactory();

        var routes = factory.Services
            .GetRequiredService<EndpointDataSource>()
            .Endpoints
            .OfType<RouteEndpoint>()
            .Select(endpoint => endpoint.RoutePattern.RawText ?? string.Empty)
            .ToList();

        Assert.NotEmpty(routes);
        Assert.DoesNotContain(routes, route =>
            route.Contains("rate", StringComparison.OrdinalIgnoreCase) ||
            route.Contains("progression", StringComparison.OrdinalIgnoreCase) ||
            route.Contains("admin", StringComparison.OrdinalIgnoreCase));
    }
}
