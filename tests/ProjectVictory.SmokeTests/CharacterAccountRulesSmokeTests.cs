using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.Session;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-CHAR-013 (PO-36 a PO-39; R-066) pela composição real (InMemory) com relógio controlado.
public sealed class CharacterAccountRulesSmokeTests
{
    private sealed class Server : IAsyncDisposable
    {
        public ServiceProvider Provider { get; }

        public Server(ManualTimeProvider clock)
        {
            var services = new ServiceCollection();
            services.AddProjectVictorySmokeTestServices();
            var databaseName = $"chars-{Guid.NewGuid():N}"; // fora do lambda: o mesmo banco em todos os escopos
            services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
            services.AddSingleton<TimeProvider>(clock);
            Provider = services.BuildServiceProvider();
        }

        public async Task<T> As<T>(Guid accountId, Func<IServiceProvider, Task<T>> action)
        {
            using var scope = Provider.CreateScope();
            scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(accountId);
            return await action(scope.ServiceProvider);
        }

        public Task<Guid> Create(Guid accountId, string name, CharacterClass characterClass, CharacterGender? gender = null, string? outfit = null) =>
            As(accountId, sp => sp.GetRequiredService<CreateCharacterHandler>().Handle(new CreateCharacterCommand(name, characterClass, gender, outfit)));

        public Task<IReadOnlyList<GetCharactersResult>> List(Guid accountId) =>
            As(accountId, sp => sp.GetRequiredService<GetCharactersHandler>().Handle(new GetCharactersQuery()));

        public Task<CharacterSlotsResult> Slots(Guid accountId) =>
            As(accountId, sp => sp.GetRequiredService<GetCharacterSlotsHandler>().Handle());

        public Task<CharacterDeletionResult> Delete(Guid accountId, Guid characterId) =>
            As(accountId, sp => sp.GetRequiredService<CharacterDeletionHandler>().RequestAsync(characterId));

        public Task<CharacterDeletionResult> Cancel(Guid accountId, Guid characterId) =>
            As(accountId, sp => sp.GetRequiredService<CharacterDeletionHandler>().CancelAsync(characterId));

        public async ValueTask DisposeAsync() => await Provider.DisposeAsync();
    }

    [Fact]
    public async Task ThreeUsableSlots_TwoLocked_FourthCreationIsRefused()
    {
        await using var server = new Server(new ManualTimeProvider());
        var account = Guid.NewGuid();

        await server.Create(account, TestNames.Unique("Ka"), CharacterClass.Knight);
        await server.Create(account, TestNames.Unique("Pa"), CharacterClass.Paladin);
        await server.Create(account, TestNames.Unique("Sa"), CharacterClass.Sorcerer);

        await Assert.ThrowsAsync<CharacterCreationConflictException>(() => server.Create(account, TestNames.Unique("Da"), CharacterClass.Druid));

        var slots = await server.Slots(account);
        Assert.Equal(5, slots.Slots.Count);
        Assert.Equal(3, slots.Slots.Count(s => s.State == CharacterSlotState.Occupied));
        Assert.Equal(2, slots.Slots.Count(s => s.State == CharacterSlotState.Locked));
        Assert.Equal(3, (await server.List(account)).Count);
    }

    [Fact]
    public async Task OnePerClass_SecondKnightIsRefused()
    {
        await using var server = new Server(new ManualTimeProvider());
        var account = Guid.NewGuid();

        await server.Create(account, TestNames.Unique("Ka"), CharacterClass.Knight);

        await Assert.ThrowsAsync<CharacterCreationConflictException>(() => server.Create(account, TestNames.Unique("Kb"), CharacterClass.Knight));
        Assert.Single(await server.List(account));
    }

    [Fact]
    public async Task Name_FormatAndUniqueness_AcrossAccounts()
    {
        await using var server = new Server(new ManualTimeProvider());
        var name = TestNames.Unique("Na");

        await Assert.ThrowsAsync<InvalidCharacterRequestException>(() => server.Create(Guid.NewGuid(), "Hero1", CharacterClass.Knight));
        await server.Create(Guid.NewGuid(), name, CharacterClass.Knight);
        await Assert.ThrowsAsync<CharacterCreationConflictException>(() => server.Create(Guid.NewGuid(), name, CharacterClass.Druid));
    }

    [Fact]
    public async Task GenderAndOutfit_AreStored_UnknownAreRefused()
    {
        await using var server = new Server(new ManualTimeProvider());
        var account = Guid.NewGuid();

        await Assert.ThrowsAsync<InvalidCharacterRequestException>(() => server.Create(account, TestNames.Unique("Ga"), CharacterClass.Knight, outfit: "Dragon"));
        await Assert.ThrowsAsync<InvalidCharacterRequestException>(() => server.Create(account, TestNames.Unique("Ga"), CharacterClass.Knight, CharacterGender.Unspecified));

        await server.Create(account, TestNames.Unique("Ga"), CharacterClass.Knight, CharacterGender.Female, "scout");
        var character = Assert.Single(await server.List(account));
        Assert.Equal("Female", character.Gender);
        Assert.Equal("Scout", character.Outfit);
        Assert.Equal("Knight", character.CharacterClass);
    }

    [Fact]
    public async Task Deletion_PendingSevenDays_FinalizedByTheServer_FreesSlotClassAndName()
    {
        var clock = new ManualTimeProvider();
        await using var server = new Server(clock);
        var account = Guid.NewGuid();
        var name = TestNames.Unique("De");
        var id = await server.Create(account, name, CharacterClass.Knight);

        var requested = await server.Delete(account, id);
        Assert.Equal(clock.GetUtcNow().AddDays(7), requested.DeletionScheduledFor);
        await Assert.ThrowsAsync<CharacterDeletionConflictException>(() => server.Delete(account, id));

        // Durante a espera: continua na conta (com a data), ocupando o slot, a classe e o nome.
        clock.Advance(TimeSpan.FromDays(7) - TimeSpan.FromSeconds(1));
        Assert.Equal(requested.DeletionScheduledFor, Assert.Single(await server.List(account)).DeletionScheduledFor);
        await Assert.ThrowsAsync<CharacterCreationConflictException>(() => server.Create(account, TestNames.Unique("Kn"), CharacterClass.Knight));

        // Venceu: o servidor finaliza; slot, classe e nome ficam livres.
        clock.Advance(TimeSpan.FromSeconds(1));
        Assert.Empty(await server.List(account));
        await Assert.ThrowsAsync<OwnershipViolationException>(() => server.Cancel(account, id));
        await server.Create(account, name, CharacterClass.Knight);
    }

    [Fact]
    public async Task Deletion_CanBeCancelled_DuringTheWait()
    {
        var clock = new ManualTimeProvider();
        await using var server = new Server(clock);
        var account = Guid.NewGuid();
        var id = await server.Create(account, TestNames.Unique("Ca"), CharacterClass.Knight);

        await server.Delete(account, id);
        clock.Advance(TimeSpan.FromDays(3));
        Assert.Null((await server.Cancel(account, id)).DeletionScheduledFor);
        await Assert.ThrowsAsync<CharacterDeletionConflictException>(() => server.Cancel(account, id));

        clock.Advance(TimeSpan.FromDays(10));
        Assert.Null(Assert.Single(await server.List(account)).DeletionScheduledFor);
    }

    [Fact]
    public async Task Deletion_OfAnotherAccountsCharacter_IsRefused()
    {
        await using var server = new Server(new ManualTimeProvider());
        var owner = Guid.NewGuid();
        var id = await server.Create(owner, TestNames.Unique("Ow"), CharacterClass.Knight);

        await Assert.ThrowsAsync<OwnershipViolationException>(() => server.Delete(Guid.NewGuid(), id));
        await Assert.ThrowsAsync<OwnershipViolationException>(() => server.Cancel(Guid.NewGuid(), id));
    }

    [Fact]
    public async Task FinalizedDeletion_EnterWithoutAnotherCharacter_FindsNoCharacter()
    {
        var clock = new ManualTimeProvider();
        await using var server = new Server(clock);
        var account = Guid.NewGuid();
        var id = await server.Create(account, TestNames.Unique("En"), CharacterClass.Knight);
        await server.Delete(account, id);
        clock.Advance(TimeSpan.FromDays(8));

        await Assert.ThrowsAnyAsync<Exception>(() =>
            server.As(account, sp => sp.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand())));
    }
}
