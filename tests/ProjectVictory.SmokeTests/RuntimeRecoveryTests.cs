using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Runtime;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Runtime;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-RUNTIME-011 (Handbook §18; ADR-057): Recovery ao iniciar o servidor — personagem + Runtime dos
// checkpoints, contexto reconstruído na tela inicial (Idle), SEM Hunt Session.
public sealed class RuntimeRecoveryTests
{
    [Fact]
    public async Task Recover_RebuildsRuntimeAtHome_FromTheCheckpoint_WithoutHunt_SkipsOrphans()
    {
        var databaseName = $"recovery-{Guid.NewGuid():N}";
        Guid characterId;
        Guid runtimeId;

        // "Primeira execução" do servidor: personagem entra e começa uma Hunt.
        {
            var services = new ServiceCollection();
            services.AddProjectVictorySmokeTestServices();
            services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
            await using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

            characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
                .Handle(new CreateCharacterCommand("R" + Guid.NewGuid().ToString("N")[..10], CharacterClass.Knight));
            runtimeId = (await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand())).RuntimeId;
            await scope.ServiceProvider.GetRequiredService<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(characterId, runtimeId));

            // Checkpoint com vida acima do máximo (ex.: valores antigos) e um Runtime órfão.
            await scope.ServiceProvider.GetRequiredService<IRuntimeRepository>().SaveCheckpointAsync(runtimeId, 99_999, 99_999);
            await scope.ServiceProvider.GetRequiredService<IRuntimeRepository>().AddAsync(Guid.NewGuid(), Guid.NewGuid(), 10, 10);
        }

        // "Reinício": memória nova (singletons novos), mesmo banco.
        {
            var services = new ServiceCollection();
            services.AddProjectVictorySmokeTestServices();
            services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
            await using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();

            var report = await scope.ServiceProvider.GetRequiredService<RuntimeRecoveryService>().RecoverAsync();

            Assert.Equal(1, report.Recovered);
            Assert.Equal(1, report.SkippedWithoutCharacter);

            var store = scope.ServiceProvider.GetRequiredService<IRuntimeExecutionStateStore>();
            Assert.True(store.TryGetByCharacterId(characterId, out var runtime));
            Assert.Equal(runtimeId, runtime.RuntimeId);
            Assert.Equal(RuntimeActivity.Idle, runtime.GameplayContext!.CurrentActivity);
            Assert.Equal(EndHuntHandler.MainCity, runtime.WorldContext);

            var character = await scope.ServiceProvider.GetRequiredService<ICharacterRepository>().GetByIdAsync(characterId);
            Assert.Equal(character!.MaximumHealth.Value, runtime.CharacterState!.CurrentHealth); // limitado ao máximo
            Assert.Equal(character.MaximumMana.Value, runtime.CharacterState.CurrentMana); // revisão: mana também limitada

            // A Hunt NÃO foi recuperada (ADR-057).
            Assert.Null(await scope.ServiceProvider.GetRequiredService<IHuntRepository>().GetRunningByCharacterIdAsync(characterId));
        }
    }
}
