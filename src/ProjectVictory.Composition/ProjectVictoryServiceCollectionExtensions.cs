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
        RewardSettings rewardSettings,
        ProjectVictory.Application.Combat.TurnBased.CombatSettings? combatSettings = null,
        ProjectVictory.Application.Supply.SupplySettings? supplySettings = null,
        ProjectVictory.Application.Progression.ILevelTitleProvider? levelTitles = null)
    {

        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(rewardSettings);

        // PV-PROG-013: títulos por Level. Padrão: FixedLevelTitleProvider (tabela do DEC-20261106-30); o host
        // pode passar outro aqui ou substituí-lo depois (a API usa Replace com o provedor de configuração).
        services.AddSingleton(levelTitles ?? ProjectVictory.Application.Progression.FixedLevelTitleProvider.Default);

        // PV-SUPPLY-010: poções (política do jogador, consumo, fim de suprimento, poções iniciais).
        services.AddSingleton(supplySettings ?? new ProjectVictory.Application.Supply.SupplySettings());
        services.AddSingleton<ProjectVictory.Application.Supply.ISupplyPolicyStore, ProjectVictory.Application.Supply.SupplyPolicyStore>();
        services.AddScoped<ProjectVictory.Application.Supply.CombatSupplyContext>();
        services.AddScoped<ProjectVictory.Application.Combat.TurnBased.ICombatActionObserver, ProjectVictory.Application.Supply.SupplyConsumptionObserver>();
        services.AddScoped<ProjectVictory.Application.Supply.SetSupplyPolicyHandler>();

        // PV-COMBAT-011: semente dos combates (fixa só por configuração do servidor, para testes).
        services.AddSingleton(combatSettings ?? new ProjectVictory.Application.Combat.TurnBased.CombatSettings());
        services.AddSingleton<ProjectVictory.Application.Combat.TurnBased.ICombatSeedProvider, ProjectVictory.Application.Combat.TurnBased.CombatSeedProvider>();

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

        // PV-COMBAT-010/011: Combat Orchestrator, ordem de turnos e seleção da Secondary Action.
        services.AddSingleton<ProjectVictory.Application.Combat.TurnBased.ITurnScheduler, ProjectVictory.Application.Combat.TurnBased.SequentialTurnScheduler>();
        services.AddScoped<ProjectVictory.Application.Combat.TurnBased.ICombatActionSelector, ProjectVictory.Application.Supply.SupplyAwareCombatActionSelector>();

        // PV-MONSTER-010 (ADR-005): Monster AI única, orientada pelos dados de cada monstro.
        services.AddSingleton<ProjectVictory.Application.Combat.TurnBased.IMonsterBehaviorProvider, ProjectVictory.Infrastructure.Combat.MonsterBehaviorProvider>();
        services.AddSingleton<ProjectVictory.Application.Combat.TurnBased.IMonsterAI, ProjectVictory.Application.Combat.TurnBased.DataDrivenMonsterAI>();

        // PV-COMBAT-012: regras de dano (fórmula PROVISÓRIA), perfis de monstro e efeitos de suprimento.
        services.AddSingleton<ProjectVictory.Domain.Combat.Rules.IDamageFormula, ProjectVictory.Domain.Combat.Rules.ProvisionalTibiaLikeDamageFormula>();
        services.AddSingleton<ProjectVictory.Application.Combat.TurnBased.IMonsterCombatProfileProvider, ProjectVictory.Infrastructure.Combat.MonsterCombatProfileProvider>();
        services.AddSingleton<ProjectVictory.Application.Combat.ISupplyEffects, ProjectVictory.Infrastructure.Combat.ProvisionalSupplyEffects>();
        services.AddScoped<ProjectVictory.Application.Combat.TurnBased.CombatantFactory>();
        services.AddScoped<ProjectVictory.Application.Combat.TurnBased.CombatOrchestrator>();
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
        // PV-CONTENT-011: área de caça (Spider Cave) definida por dados.
        services.AddSingleton<IHuntAreaCatalog, ProjectVictory.Infrastructure.Content.SpiderCaveHuntArea>();
        services.AddScoped<EncounterGenerator>(provider => new EncounterGenerator(provider.GetRequiredService<IHuntAreaCatalog>()));
        services.AddScoped<ExecuteHuntHandler>();
        services.AddScoped<StartHuntSessionHandler>();
        services.AddScoped<StopHuntSessionHandler>();
        services.AddScoped<FinalizeHuntAfterCombatHandler>();
        services.AddScoped<ForwardHuntRewardHandler>();
        services.AddScoped<CompleteHuntRewardFlowHandler>();
        services.AddScoped<GameplayResultPipeline>();
        services.AddScoped<AttackEncounterHandler>();

        // PV-HUNT-010: pipeline único de encerramento da Hunt (ADR-012).
        services.AddScoped<EndHuntHandler>();

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
