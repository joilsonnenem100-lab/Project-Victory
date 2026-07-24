using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Account;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

public sealed class CoreGameplayFlowSmokeTest
{
    [Fact]
    public async Task Core_Gameplay_Flow_Should_Complete()
{
    SmokeTestReporter.Start();

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



        

        var createRuntime =
            scope.ServiceProvider
                .GetRequiredService<CreateRuntimeExecutionHandler>();


        var runtimeId =
            Guid.NewGuid();


        var runtimeResult =
            await createRuntime.Handle(
                new CreateRuntimeExecutionCommand(
                    runtimeId,
                    characterId));


        Assert.Equal(
            runtimeId,
            runtimeResult.RuntimeId);


        Assert.Equal(
            characterId,
            runtimeResult.CharacterId);



      

        var executeHunt =
            scope.ServiceProvider
                .GetRequiredService<ExecuteHuntHandler>();


        var huntResult =
            await executeHunt.Handle(
                new ExecuteHuntCommand(
                    characterId,
                    runtimeId));


        Assert.NotEqual(
            Guid.Empty,
            huntResult.HuntId);


        Assert.NotNull(
            huntResult.Encounter);



        
        

var createCombatContext =
    scope.ServiceProvider
        .GetRequiredService<CreateCombatContextHandler>();

var combatContext =
    await createCombatContext.Handle(
        new CreateCombatContextCommand(
            huntResult.HuntId,
            characterId,
            huntResult.Encounter.CurrentTargetId));

Assert.NotEqual(
    Guid.Empty,
    combatContext.CombatContextId);

var executeCombat =
    scope.ServiceProvider
        .GetRequiredService<ExecuteCombatActionHandler>();

var actionId = Guid.NewGuid();

var combatResult =
    await executeCombat.Handle(
        new ExecuteCombatActionCommand(
            combatContext.CombatContextId,
            actionId));

Assert.True(combatResult.Processed);
Assert.Equal(combatContext.CombatContextId, combatResult.CombatContextId);



var applyReward =
    scope.ServiceProvider
        .GetRequiredService<ApplyRewardHandler>();

var rewardRequest =
    new RewardRequest(
        characterId,
        combatResult);

var rewardResult =
    await applyReward.Handle(
        new ApplyRewardCommand(
            characterId,
            rewardRequest));

Assert.True(rewardResult.Applied);
Assert.Equal(characterId, rewardResult.CharacterId);


   }
}
