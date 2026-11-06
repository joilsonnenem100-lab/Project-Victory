using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Supply;
using ProjectVictory.Persistence.Database;
using ProjectVictory.Persistence.Supply;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-PERSIST-010: a política de poções é Permanente (EF Core) e mantém a semântica do
// DEC-20261106-03 (pendente → ativa só no próximo turno).
public sealed class SupplyPolicyPersistenceTests
{
    private static ProjectVictoryDbContext NewContext(string database) =>
        new(new DbContextOptionsBuilder<ProjectVictoryDbContext>().UseInMemoryDatabase(database).Options);

    [Fact]
    public void PendingBecomesActive_OnlyOnActivation_AndSurvivesANewContext()
    {
        var database = "supply-" + Guid.NewGuid().ToString("N");
        var characterId = Guid.NewGuid();

        using (var context = NewContext(database))
        {
            var store = new PersistentSupplyPolicyStore(context);
            Assert.Equal(SupplyPolicy.None, store.GetActive(characterId));

            store.SetPending(characterId, new SupplyPolicy(40, 30));
            Assert.Equal(SupplyPolicy.None, store.GetActive(characterId)); // ainda não vale
        }

        // Outro contexto (como outra requisição/processo): a pendente foi gravada.
        using (var context = NewContext(database))
        {
            var store = new PersistentSupplyPolicyStore(context);
            Assert.Equal(new SupplyPolicy(40, 30), store.ActivatePending(characterId));
        }

        using (var context = NewContext(database))
        {
            var store = new PersistentSupplyPolicyStore(context);
            Assert.Equal(new SupplyPolicy(40, 30), store.GetActive(characterId));
            Assert.Equal(new SupplyPolicy(40, 30), store.ActivatePending(characterId)); // sem pendente: mantém
            Assert.False(context.SupplyPolicies.Single().HasPending);
        }
    }

    [Fact]
    public void Composition_UsesThePersistentStore()
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        Assert.IsType<PersistentSupplyPolicyStore>(scope.ServiceProvider.GetRequiredService<ISupplyPolicyStore>());
    }
}
