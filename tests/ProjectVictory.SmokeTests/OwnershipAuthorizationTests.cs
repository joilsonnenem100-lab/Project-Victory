using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Inventory;
using ProjectVictory.Application.Session;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-SEC-001: o dono age sobre os próprios recursos; outra conta recebe OwnershipViolationException.
public sealed class OwnershipAuthorizationTests
{
    private static async Task<(ServiceProvider Provider, IServiceScope Scope, TestCurrentPlayerContext Player)> CreateAsync()
    {
        var services = new ServiceCollection();

        services.AddProjectVictorySmokeTestServices();

        var provider =
            services.BuildServiceProvider();

        var scope =
            provider.CreateScope();

        var player =
            scope.ServiceProvider
                .GetRequiredService<TestCurrentPlayerContext>();

        await Task.CompletedTask;

        return (provider, scope, player);
    }


    private static async Task<Guid> CreateOwnedCharacterWithRuntimeAsync(
        IServiceProvider services,
        TestCurrentPlayerContext player,
        Guid accountId)
    {
        player.SetAccount(accountId);

        var characterId =
            await services
                .GetRequiredService<CreateCharacterHandler>()
                .Handle(
                    new CreateCharacterCommand(
                        TestNames.Unique("Owner"),
                        CharacterClass.Knight));

        await services
            .GetRequiredService<CreateRuntimeExecutionHandler>()
            .Handle(
                new CreateRuntimeExecutionCommand(
                    Guid.NewGuid(),
                    characterId));

        return characterId;
    }


    [Fact]
    public async Task StartHuntSession_Owner_Succeeds_OtherAccount_IsDenied()
    {
        var (provider, scope, player) = await CreateAsync();
        await using var _ = provider;
        using var __ = scope;

        var ownerAccount = Guid.NewGuid();

        var characterId =
            await CreateOwnedCharacterWithRuntimeAsync(
                scope.ServiceProvider,
                player,
                ownerAccount);

        var start =
            scope.ServiceProvider
                .GetRequiredService<StartHuntSessionHandler>();

        var ownResult =
            await start.Handle(
                new StartHuntSessionCommand(characterId));

        Assert.Equal(characterId, ownResult.CharacterId);

        player.SetAccount(Guid.NewGuid());

        await Assert.ThrowsAsync<OwnershipViolationException>(
            () => start.Handle(
                new StartHuntSessionCommand(characterId)));
    }


    [Fact]
    public async Task StopHuntSession_OtherAccount_IsDenied()
    {
        var (provider, scope, player) = await CreateAsync();
        await using var _ = provider;
        using var __ = scope;

        var characterId =
            await CreateOwnedCharacterWithRuntimeAsync(
                scope.ServiceProvider,
                player,
                Guid.NewGuid());

        var session =
            await scope.ServiceProvider
                .GetRequiredService<StartHuntSessionHandler>()
                .Handle(
                    new StartHuntSessionCommand(characterId));

        player.SetAccount(Guid.NewGuid());

        await Assert.ThrowsAsync<OwnershipViolationException>(
            () => scope.ServiceProvider
                .GetRequiredService<StopHuntSessionHandler>()
                .Handle(
                    new StopHuntSessionCommand(session.HuntSessionId)));
    }


    [Fact]
    public async Task GetInventory_Owner_Succeeds_OtherAccount_IsDenied()
    {
        var (provider, scope, player) = await CreateAsync();
        await using var _ = provider;
        using var __ = scope;

        var ownerAccount = Guid.NewGuid();

        var characterId =
            await CreateOwnedCharacterWithRuntimeAsync(
                scope.ServiceProvider,
                player,
                ownerAccount);

        var character =
            await scope.ServiceProvider
                .GetRequiredService<ICharacterRepository>()
                .GetByIdAsync(characterId);

        Assert.NotNull(character);

        var inventory =
            scope.ServiceProvider
                .GetRequiredService<GetCharacterInventoryHandler>();

        var ownResult =
            inventory.Handle(
                new GetCharacterInventoryQuery(characterId),
                character);

        Assert.True(ownResult.Success);

        player.SetAccount(Guid.NewGuid());

        Assert.Throws<OwnershipViolationException>(
            () => inventory.Handle(
                new GetCharacterInventoryQuery(characterId),
                character));
    }


    [Fact]
    public async Task StopHuntSession_Owner_Succeeds()
    {
        var (provider, scope, player) = await CreateAsync();
        await using var _ = provider;
        using var __ = scope;

        var characterId =
            await CreateOwnedCharacterWithRuntimeAsync(
                scope.ServiceProvider,
                player,
                Guid.NewGuid());

        var session =
            await scope.ServiceProvider
                .GetRequiredService<StartHuntSessionHandler>()
                .Handle(
                    new StartHuntSessionCommand(characterId));

        await scope.ServiceProvider
            .GetRequiredService<StopHuntSessionHandler>()
            .Handle(
                new StopHuntSessionCommand(session.HuntSessionId));
    }


    [Fact]
    public async Task ExecuteHunt_OtherAccount_IsDenied()
    {
        var (provider, scope, player) = await CreateAsync();
        await using var _ = provider;
        using var __ = scope;

        var characterId =
            await CreateOwnedCharacterWithRuntimeAsync(
                scope.ServiceProvider,
                player,
                Guid.NewGuid());

        player.SetAccount(Guid.NewGuid());

        await Assert.ThrowsAsync<OwnershipViolationException>(
            () => scope.ServiceProvider
                .GetRequiredService<ExecuteHuntHandler>()
                .Handle(
                    new ExecuteHuntCommand(
                        characterId,
                        Guid.NewGuid())));
    }


    [Fact]
    public async Task OwnershipGuard_Runtime_Owner_Succeeds_OtherAccount_IsDenied()
    {
        var (provider, scope, player) = await CreateAsync();
        await using var _ = provider;
        using var __ = scope;

        var ownerAccount = Guid.NewGuid();

        var characterId =
            await CreateOwnedCharacterWithRuntimeAsync(
                scope.ServiceProvider,
                player,
                ownerAccount);

        var runtimeId =
            scope.ServiceProvider
                .GetRequiredService<IRuntimeExecutionStateStore>()
                .GetByCharacterId(characterId)
                .RuntimeId;

        var guard =
            scope.ServiceProvider
                .GetRequiredService<PlayerOwnershipGuard>();

        await guard.EnsureRuntimeAsync(runtimeId);

        player.SetAccount(Guid.NewGuid());

        await Assert.ThrowsAsync<OwnershipViolationException>(
            () => guard.EnsureRuntimeAsync(runtimeId));
    }


    [Fact]
    public async Task OwnershipGuard_Encounter_Owner_Succeeds_OtherAccount_IsDenied()
    {
        var (provider, scope, player) = await CreateAsync();
        await using var _ = provider;
        using var __ = scope;

        var ownerAccount = Guid.NewGuid();

        var characterId =
            await CreateOwnedCharacterWithRuntimeAsync(
                scope.ServiceProvider,
                player,
                ownerAccount);

        var hunt =
            await scope.ServiceProvider
                .GetRequiredService<ExecuteHuntHandler>()
                .Handle(
                    new ExecuteHuntCommand(
                        characterId,
                        Guid.NewGuid()));

        var guard =
            scope.ServiceProvider
                .GetRequiredService<PlayerOwnershipGuard>();

        await guard.EnsureEncounterAsync(hunt.Encounter.Id);

        player.SetAccount(Guid.NewGuid());

        await Assert.ThrowsAsync<OwnershipViolationException>(
            () => guard.EnsureEncounterAsync(hunt.Encounter.Id));
    }


    [Fact]
    public async Task OwnershipGuard_UnknownCharacter_IsDenied()
    {
        var (provider, scope, player) = await CreateAsync();
        await using var _ = provider;
        using var __ = scope;

        player.SetAccount(Guid.NewGuid());

        await Assert.ThrowsAsync<OwnershipViolationException>(
            () => scope.ServiceProvider
                .GetRequiredService<PlayerOwnershipGuard>()
                .EnsureCharacterAsync(Guid.NewGuid()));
    }
}
