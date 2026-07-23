using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Account;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Persistence.Database;
using ProjectVictory.Domain.Character;
using Xunit;

namespace ProjectVictory.SmokeTests;

public sealed class CoreGameplayFlowSmokeTest
{
    [Fact]
    public async Task Core_Gameplay_Flow_Should_Complete()
    {
        var services = new ServiceCollection();

        services.AddProjectVictorySmokeTestServices();

        await using var provider =
            services.BuildServiceProvider();

        using var scope =
            provider.CreateScope();


        var db =
            scope.ServiceProvider
                .GetRequiredService<ProjectVictoryDbContext>();

        await db.Database.EnsureCreatedAsync();


        var createAccount =
            scope.ServiceProvider
                .GetRequiredService<CreateAccountHandler>();

        var login =
            scope.ServiceProvider
                .GetRequiredService<LoginHandler>();


        var email =
            $"smoke.{Guid.NewGuid()}@projectvictory.com";

        const string password =
            "SmokeTest@123";


        var accountId =
            await createAccount.Handle(
                new CreateAccountCommand(
                    email,
                    password));


        Assert.NotEqual(
            Guid.Empty,
            accountId);


        var loggedAccountId =
            await login.Handle(
                new LoginCommand(
                    email,
                    password));


        Assert.Equal(
            accountId,
            loggedAccountId);


        var currentPlayer =
            scope.ServiceProvider
                .GetRequiredService<TestCurrentPlayerContext>();

        currentPlayer.SetAccount(
            loggedAccountId);


        // PASSO 4 — Character Creation

        var createCharacter =
            scope.ServiceProvider
                .GetRequiredService<CreateCharacterHandler>();


        var characterId =
            await createCharacter.Handle(
                new CreateCharacterCommand(
                    "SmokeHero",
                    CharacterClass.Knight));


        Assert.NotEqual(
            Guid.Empty,
            characterId);


        // Próximas etapas:
        // Runtime
        // Hunt
        // Combat
        // Reward
        // Experience


        var runtime =
            scope.ServiceProvider
                .GetRequiredService<CreateRuntimeExecutionHandler>();

        Assert.NotNull(runtime);


        var hunt =
            scope.ServiceProvider
                .GetRequiredService<ExecuteHuntHandler>();

        Assert.NotNull(hunt);


        var attack =
            scope.ServiceProvider
                .GetRequiredService<AttackEncounterHandler>();

        Assert.NotNull(attack);
    }
}