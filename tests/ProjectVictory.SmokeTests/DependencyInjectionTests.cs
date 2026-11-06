using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-QA-001 parte 2: evita a defasagem já registrada (faltou IRuntimeRepository no DI de teste).
// Todo serviço registrado NO DI DE TESTE precisa ser construível, com escopos válidos: quando um
// handler já registrado aqui ganha dependência nova e o DI de teste não acompanha, este teste falha.
// PV-ARCH-001: desde então o DI de teste usa o mesmo AddProjectVictoryCore do Program.cs, então
// este teste também valida a composição real da API (R-014 resolvida).
// Os dublês (TestRewardProcessor, FixedRandomSource) sobrescrevem RewardProcessor e SystemRandomSource:
// GetRequiredService resolve só o último registro; as implementações de produção são validadas pelo
// ValidateOnBuild abaixo e pela API real no Api.Tests (revisão do PV-ARCH-001).
public sealed class DependencyInjectionTests
{
    [Fact]
    public void SmokeTestContainer_AllRegisteredServices_Resolve()
    {
        var services = new ServiceCollection();

        services.AddProjectVictorySmokeTestServices();

        using var provider =
            services.BuildServiceProvider(
                new ServiceProviderOptions
                {
                    ValidateOnBuild = true,
                    ValidateScopes = true
                });

        using var scope =
            provider.CreateScope();

        foreach (var descriptor in services)
        {
            if (descriptor.ServiceType.IsGenericTypeDefinition)
            {
                continue;
            }

            Assert.NotNull(
                scope.ServiceProvider.GetRequiredService(
                    descriptor.ServiceType));
        }
    }
}
