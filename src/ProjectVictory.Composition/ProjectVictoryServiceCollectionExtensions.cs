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
        // PV-PERSIST-010: a política de poções é configuração do jogador (Permanente) → EF Core.
        // O SupplyPolicyStore em memória continua existindo (testes de unidade).
        services.AddScoped<ProjectVictory.Application.Supply.ISupplyPolicyStore, ProjectVictory.Persistence.Supply.PersistentSupplyPolicyStore>();
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

        // PV-ADMIN-010 (DEC-20261106-12): XP rate alterado pelo administrador (persistido) e XP rate efetivo
        // usado pelo Reward Pipeline; papel de administrador pela configuração (padrão: ninguém; o host substitui).
        services.AddScoped<IExperienceRateStore, ProjectVictory.Persistence.Admin.ExperienceRateStore>();
        services.AddScoped<IExperienceRateProvider, ExperienceRateProvider>();
        Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.TryAddSingleton(
            services, ProjectVictory.Application.Admin.AdminSettings.Default);
        services.AddScoped<ProjectVictory.Application.Admin.IAccountRoleResolver, ProjectVictory.Application.Admin.ConfiguredAccountRoleResolver>();
        services.AddScoped<ProjectVictory.Application.Admin.GetExperienceRateHandler>();
        services.AddScoped<ProjectVictory.Application.Admin.SetExperienceRateHandler>();

        // Loot, Reward e Progression.
        services.AddScoped<LootResolutionHandler>();
        services.AddScoped<ApplyLootToCharacterInventoryHandler>();
        services.AddScoped<GetCharacterInventoryHandler>();
        services.AddScoped<ApplyResolvedRewardToCharacterHandler>();
        services.AddScoped<RewardResolutionHandler>();

        // PV-RUNTIME-010 (ADR-057 v1.2): sessão de jogo — presença em memória (thread-safe), heartbeat
        // HTTP e Runtime Coordinator (detecção de queda). Tempos em configuração (o host substitui).
        Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.TryAddSingleton(
            services, ProjectVictory.Application.GameSession.GameSessionSettings.Default);
        services.AddSingleton<ProjectVictory.Application.GameSession.IPresenceTracker, ProjectVictory.Application.GameSession.PresenceTracker>();
        services.AddScoped<ProjectVictory.Application.GameSession.IGameActivity, ProjectVictory.Application.GameSession.GameActivity>();
        // PV-EVENT-012 (R-060): um escopo de DI por queda (o coordenador usa o construtor com a fábrica).
        services.AddSingleton<ProjectVictory.Application.GameSession.IDisconnectScopeFactory, ProjectVictory.Composition.ServiceScopeDisconnectScopeFactory>();
        services.AddScoped(provider => new ProjectVictory.Application.GameSession.GameSessionCoordinator(
            provider.GetRequiredService<ProjectVictory.Application.GameSession.IPresenceTracker>(),
            provider.GetRequiredService<ProjectVictory.Application.GameSession.GameSessionSettings>(),
            provider.GetRequiredService<TimeProvider>(),
            // Revisão do PV-EVENT-012 (ressalva 4): os handlers vêm do escopo de cada queda, não daqui.
            Array.Empty<ProjectVictory.Application.GameSession.IDisconnectHandler>(),
            provider.GetRequiredService<ProjectVictory.Application.GameSession.IDisconnectScopeFactory>()));
        services.AddScoped<ProjectVictory.Application.GameSession.HeartbeatHandler>();

        // PV-PERSIST-011 (Handbook §19): checkpoints em início, recompensa, conclusão e logout; o
        // consumo de poções por rodada fica pendente em memória (Temporário) até o próximo checkpoint.
        services.AddSingleton<ProjectVictory.Application.Checkpoint.IPendingSupplyConsumption, ProjectVictory.Application.Checkpoint.PendingSupplyConsumption>();
        services.AddSingleton<ProjectVictory.Application.Checkpoint.ICheckpointLog, ProjectVictory.Application.Checkpoint.CheckpointLog>();
        services.AddScoped<ProjectVictory.Application.Checkpoint.ICheckpointService, ProjectVictory.Application.Checkpoint.CheckpointService>();

        // PV-XP-010 (ADR-007): Experience Distribution (regra de grupo PROVISÓRIA: divisão igual).
        services.AddSingleton<IExperienceDistributor, EvenSplitExperienceDistributor>();

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
        // PV-SPAWN-010 (ADR-009/010/047): Spawn System stateless (singleton, sem estado) dirigido pela Hunt.
        // Revisão do PV-SPAWN-010: relógio explícito (o host pode trocar por outro antes).
        Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.TryAddSingleton(services, TimeProvider.System);
        // PV-EVENT-011: o Spawn System publica o ciclo de vida no Event Bus (escopo da requisição).
        services.AddScoped<ProjectVictory.Application.Spawn.ISpawnSystem>(provider =>
            new ProjectVictory.Application.Spawn.StatelessSpawnSystem(
                provider.GetRequiredService<TimeProvider>(),
                provider.GetRequiredService<ProjectVictory.Application.Events.IEventBus>()));

        // PV-EVENT-010/011 (ADR-049/050): Event Bus tipado (só Domain Events) e seus handlers.
        services.AddScoped<ProjectVictory.Application.Events.IEventBus, ProjectVictory.Application.Events.InProcessEventBus>();
        // PV-EVENT-012 (ADR-058): destino das falhas de assinantes isolados (notificações pós-commit).
        services.AddSingleton<ProjectVictory.Composition.LoggingEventFailureSink>();
        services.AddSingleton<ProjectVictory.Application.Events.IEventFailureSink>(provider =>
            provider.GetRequiredService<ProjectVictory.Composition.LoggingEventFailureSink>());
        services.AddScoped<ProjectVictory.Application.Events.IAsyncEventHandler<ProjectVictory.Domain.Events.CombatCompletedEvent>, ProjectVictory.Application.Reward.RewardOnCombatCompleted>();
        services.AddSingleton<ProjectVictory.Application.Spawn.SpawnLifecycleCounter>();
        services.AddSingleton<ProjectVictory.Application.Events.IEventHandler<ProjectVictory.Domain.Events.SpawnInstanceActivatedEvent>>(
            provider => provider.GetRequiredService<ProjectVictory.Application.Spawn.SpawnLifecycleCounter>());
        services.AddSingleton<ProjectVictory.Application.Events.IEventHandler<ProjectVictory.Domain.Events.SpawnInstanceDespawnedEvent>>(
            provider => provider.GetRequiredService<ProjectVictory.Application.Spawn.SpawnLifecycleCounter>());
        // PV-HUNT-011: Hunt Loop — locks por Hunt e Spawn Instances ativas (despawn no fim, R-044).
        services.AddSingleton<IHuntLock, HuntLock>();
        // PV-HUNT-013: portão de início de Hunt por personagem (DEC-20261106-46).
        services.AddSingleton<IHuntStartGate, HuntStartGate>();
        // PV-RUNTIME-013: marca de logout (R-054).
        services.AddScoped<ProjectVictory.Application.GameSession.LoggedOutMarker>();
        services.AddSingleton<IActiveSpawnRegistry, ActiveSpawnRegistry>();
        services.AddScoped<EncounterGenerator>(provider => new EncounterGenerator(
            provider.GetRequiredService<IHuntAreaCatalog>(),
            provider.GetRequiredService<ProjectVictory.Application.Spawn.ISpawnSystem>(),
            provider.GetRequiredService<IActiveSpawnRegistry>()));
        services.AddScoped<LeaveHuntHandler>();

        // PV-HUNT-012: queda (> 1 min) encerra a Hunt pelo pipeline único; logout explícito.
        services.AddScoped<ProjectVictory.Application.GameSession.IDisconnectHandler, DisconnectHuntHandler>();
        services.AddScoped<LogoutHandler>();

        // PV-RUNTIME-011: Recovery do Runtime ao iniciar o servidor (sem Hunt, ADR-057).
        services.AddScoped<ProjectVictory.Application.Runtime.RuntimeRecoveryService>();
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

        // PV-HUD-ANALYZER-010: Hunt Analyzer (apuração em memória, histórico persistido pelo assinante
        // do HuntEndedEvent, consultas). Tamanho do histórico PROVISÓRIO (o host pode substituir).
        Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.TryAddSingleton(
            services, ProjectVictory.Application.HuntAnalyzer.HuntAnalyzerSettings.Default);
        services.AddSingleton<ProjectVictory.Application.HuntAnalyzer.IHuntAnalyzerTally, ProjectVictory.Application.HuntAnalyzer.InMemoryHuntAnalyzerTally>();
        services.AddScoped<IRewardDeliveryObserver, ProjectVictory.Application.HuntAnalyzer.HuntAnalyzerLootObserver>();
        // PV-HUD-ANALYZER-010 R1: falha do observador isolada e registrada no log (não derruba o Reward).
        services.AddSingleton<ProjectVictory.Composition.LoggingRewardObserverFailureSink>();
        services.AddSingleton<ProjectVictory.Application.Reward.IRewardObserverFailureSink>(provider =>
            provider.GetRequiredService<ProjectVictory.Composition.LoggingRewardObserverFailureSink>());
        services.AddScoped<ProjectVictory.Application.HuntAnalyzer.HuntAnalyzerPricing>();
        services.AddScoped<ProjectVictory.Application.HuntAnalyzer.IHuntAnalyzerHistoryStore, ProjectVictory.Persistence.HuntAnalyzer.HuntAnalyzerHistoryStore>();
        services.AddScoped<ProjectVictory.Application.HuntAnalyzer.GetHuntAnalyzerHandler>();
        services.AddScoped<ProjectVictory.Application.HuntAnalyzer.GetHuntAnalyzerHistoryHandler>();
        services.AddScoped<ProjectVictory.Application.Events.IAsyncEventHandler<ProjectVictory.Domain.Events.HuntEndedEvent>, ProjectVictory.Application.HuntAnalyzer.HuntAnalyzerOnHuntEnded>();

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
