using ProjectVictory.Domain.Combat;
using ProjectVictory.Persistence.Combat;
using ProjectVictory.Persistence.Hunt;
using Xunit;
using HuntAggregate = ProjectVictory.Domain.Hunt.Hunt;

namespace ProjectVictory.SmokeTests;

// PV-STATE-001: stores em memória registrados como singleton precisam suportar
// requisições concorrentes (antes: List<> sem sincronização).
public sealed class InMemoryStateStoreTests
{
    private const int Operations = 2000;

    [Fact]
    public async Task HuntRepository_ConcurrentAdds_AreAllStoredAndReadable()
    {
        var repository = new HuntRepository();
        var hunts = Enumerable.Range(0, Operations)
            .Select(_ => new HuntAggregate(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()))
            .ToArray();

        await Parallel.ForEachAsync(hunts, async (hunt, ct) =>
        {
            await repository.AddAsync(hunt, ct);
            Assert.Same(hunt, await repository.GetByIdAsync(hunt.Id, ct));
        });

        foreach (var hunt in hunts)
        {
            Assert.Same(hunt, await repository.GetByIdAsync(hunt.Id));
        }
    }

    [Fact]
    public async Task HuntRepository_DuplicateId_IsRejected_AndUnknownIsNull()
    {
        var repository = new HuntRepository();
        var hunt = new HuntAggregate(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        await repository.AddAsync(hunt);

        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.AddAsync(hunt));
        Assert.Null(await repository.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CombatContextRepository_ConcurrentAdds_AreAllStoredAndReadable()
    {
        var repository = new CombatContextRepository();
        var contexts = Enumerable.Range(0, Operations)
            .Select(_ => new CombatContext(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()))
            .ToArray();

        await Parallel.ForEachAsync(contexts, async (context, ct) =>
        {
            await repository.AddAsync(context, ct);
        });

        foreach (var context in contexts)
        {
            Assert.Same(context, await repository.GetByIdAsync(context.Id));
        }

        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.AddAsync(contexts[0]));
    }
}
