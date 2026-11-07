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

            var ex = Assert.ThrowsAny<Exception>(() => factory.Services.GetRequiredService<RewardSettings>());

            // Ressalva da revisão do PV-REWARD-010: a falha tem de vir da validação do rate.
            Assert.True(
                Flatten(ex).OfType<ArgumentOutOfRangeException>().Any(e => e.ParamName == "value"),
                ex.ToString());
        }
        finally
        {
            Environment.SetEnvironmentVariable(RateVariable, previous);
        }
    }

    [Fact]
    public void NonNumericRate_FailsStartup()
    {
        var previous = Environment.GetEnvironmentVariable(RateVariable);
        Environment.SetEnvironmentVariable(RateVariable, "abc");

        try
        {
            using var factory = new ApiFactory();

            var ex = Assert.ThrowsAny<Exception>(() => factory.Services.GetRequiredService<RewardSettings>());

            Assert.Contains(Flatten(ex), e => e.Message.Contains(RewardSettings.ExperienceRateKey, StringComparison.Ordinal));
        }
        finally
        {
            Environment.SetEnvironmentVariable(RateVariable, previous);
        }
    }

    private static IEnumerable<Exception> Flatten(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            yield return current;
        }
    }

    // PV-ADMIN-010: no M2 nenhum endpoint expunha o rate. O DEC-20261106-12 prevê, no M8, a ferramenta de
    // administração: agora a ÚNICA rota com "rate"/"admin" é api/admin/xp-rate, protegida pela policy
    // "Administrator"; nenhuma rota com "progression".
    [Fact]
    public void NoEndpoint_ExposesExperienceRate()
    {
        using var factory = new ApiFactory();

        var endpoints = factory.Services
            .GetRequiredService<EndpointDataSource>()
            .Endpoints
            .OfType<RouteEndpoint>()
            .ToList();

        var routes = endpoints
            .Select(endpoint => endpoint.RoutePattern.RawText ?? string.Empty)
            .ToList();

        Assert.NotEmpty(routes);
        Assert.DoesNotContain(routes, route =>
            route.Contains("progression", StringComparison.OrdinalIgnoreCase));

        var exposing = endpoints
            .Where(endpoint =>
            {
                var route = endpoint.RoutePattern.RawText ?? string.Empty;
                return route.Contains("rate", StringComparison.OrdinalIgnoreCase) ||
                       route.Contains("admin", StringComparison.OrdinalIgnoreCase);
            })
            .ToList();

        Assert.NotEmpty(exposing);
        Assert.All(exposing, endpoint =>
        {
            Assert.Equal("api/admin/xp-rate", endpoint.RoutePattern.RawText);
            Assert.Contains(
                endpoint.Metadata.OfType<Microsoft.AspNetCore.Authorization.IAuthorizeData>(),
                data => data.Policy == ProjectVictory.API.Authentication.AdministratorAuthorizationHandler.PolicyName);
        });
    }
}
