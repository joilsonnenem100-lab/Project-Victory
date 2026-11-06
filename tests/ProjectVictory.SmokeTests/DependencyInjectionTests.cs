using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-QA-001 parte 2: evita a defasagem já registrada (faltou IRuntimeRepository no DI de teste).
// Todo serviço registrado no container do SmokeTests precisa ser construível, com escopos válidos.
// Quando um handler ganha dependência nova e o DI de teste não acompanha, este teste falha.
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
