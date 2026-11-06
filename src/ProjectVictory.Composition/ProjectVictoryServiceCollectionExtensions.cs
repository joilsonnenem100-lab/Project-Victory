using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Account;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Encounter;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.Gameplay;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Inventory;
using ProjectVictory.Application.Item;
using ProjectVictory.Application.Loot;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Runtime;
using ProjectVictory.Application.Session;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Application.World;
using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Items;
using ProjectVictory.Domain.Loot;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.World;
using ProjectVictory.Infrastructure.Loot;
using ProjectVictory.Infrastructure.Services;
using ProjectVictory.Persistence.Account;
using ProjectVictory.Persistence.Character;
using ProjectVictory.Persistence.Combat;
using ProjectVictory.Persistence.Hunt;
using ProjectVictory.Persistence.Item;
using ProjectVictory.Persistence.Runtime;

namespace ProjectVictory.Composition;

// PV-ARCH-001 (R-014): ÚNICO ponto de composição dos serviços do jogo, usado pelo Program.cs
// da API e pelos testes (SmokeTests). Antes, os testes mantinham uma cópia à mão que já ficou
// defasada duas vezes.
//
// Fica FORA daqui o que é próprio de cada host:
// - ProjectVictoryDbContext (SQLite na API; InMemory nos testes);
// - ICurrentPlayerContext (token HTTP na API; contexto fixo nos testes);
// - autenticação JWT, CORS, controllers e tratamento de erro HTTP (só a API).
// A configuração do servidor que os serviços usam (RewardSettings) é lida pelo host e passada aqui.
public static class ProjectVictoryServiceCollectionExtensions
{
    public static IServiceCollection AddProjectVictoryCore(
        this IServiceCollection services,
        RewardSettings rewardSettings)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(rewardSettings);

        // PV-SEC-001: verificação de posse de recursos do jogador.
        services.AddScoped<PlayerOwnershipGuard>();

        // Repositórios e stores (classificação em docs/contracts/active/PV-STATE-001.md).
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICharacterRepository, CharacterRepository>();
        services.AddScoped<IRuntimeRepository, RuntimeRepository>();
        services.AddSingleton<IHuntRepository, HuntRepository>();
        services.AddSingleton<ICombatContextRepository, CombatContextRepository>();
        services.AddScoped<IItemDefinitionRepository, ItemDefinitionRepository>();
        services.AddScoped<IHuntSessionRepository, HuntSessionRepository>();
        services.AddScoped<ICombatResultStore, CombatResultStore>();
        services.AddSingleton<IEncounterStateStore, EncounterStateStore>();
        services.AddSingleton<IRuntimeExecutionStateStore, RuntimeExecutionStateStore>();

        // Serviços de infraestrutura.
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ICombatResolver, CombatResolver>();
        services.AddScoped<IRewardProcessor, RewardProcessor>();
        services.AddScoped<IProgressionProcessor, ProgressionProcessor>();
        services.AddSingleton<ILootDefinitionProvider, MonsterLootDefinitionProvider>();

        // PV-REWARD-010: XP rate do servidor (DEC-20261106-12) e aleatoriedade do servidor.
        services.AddSingleton(rewardSettings);
        services.AddSingleton<IRandomSource, SystemRandomSource>();

        // Loot, Reward e Progression.
        services.AddScoped<LootResolutionHandler>();
        services.AddScoped<ApplyLootToCharacterInventoryHandler>();
        services.AddScoped<GetCharacterInventoryHandler>();
        services.AddScoped<ApplyResolvedRewardToCharacterHandler>();
        services.AddScoped<RewardResolutionHandler>();

        // Account, Character, Game.
        services.AddScoped<CreateAccountHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<CreateCharacterHandler>();
        services.AddScoped<GetCharactersHandler>();
        services.AddScoped<EnterGameHandler>();
        services.AddScoped<GetItemDefinitionHandler>();

        // Hunt.
        services.AddScoped<EncounterGenerator>();
        services.AddScoped<ExecuteHuntHandler>();
        services.AddScoped<StartHuntSessionHandler>();
        services.AddScoped<StopHuntSessionHandler>();
        services.AddScoped<FinalizeHuntAfterCombatHandler>();
        services.AddScoped<ForwardHuntRewardHandler>();
        services.AddScoped<CompleteHuntRewardFlowHandler>();
        services.AddScoped<GameplayResultPipeline>();
        services.AddScoped<AttackEncounterHandler>();

        // Runtime, World e Gameplay.
        services.AddScoped<CreateRuntimeExecutionHandler>();
        services.AddScoped<UpdateRuntimeActivityHandler>();
        services.AddScoped<ChangeRuntimeLocationHandler>();
        services.AddScoped<ResolveLocationActionsHandler>();
        services.AddScoped<BindEncounterToCombatHandler>();
        services.AddScoped<StartEncounterCombatHandler>();
        services.AddScoped<CreateCombatContextHandler>();
        services.AddScoped<ResolveCombatHandler>();
        services.AddScoped<GameplayActionDispatcher>();
        services.AddScoped<GameplayActionExecutionHandler>();
        services.AddSingleton<ILocationActionCatalog, LocationActionCatalog>();
        services.AddSingleton<IWorldDefinitionStore, WorldDefinitionStore>();

        var worldSeed =
            WorldDefinitionSeed.Create();

        services.AddSingleton<IWorldDefinitionCatalog>(
            new WorldDefinitionCatalog(
                worldSeed.Worlds,
                worldSeed.Regions,
                worldSeed.Locations));

        return services;
    }
}
