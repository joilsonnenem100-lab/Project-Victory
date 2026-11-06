using ProjectVictory.Domain.Combat;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Loot;
using ProjectVictory.Domain.Loot;

namespace ProjectVictory.Application.Reward;

public sealed class RewardResolutionHandler
{
    private readonly ApplyResolvedRewardToCharacterHandler _progressionHandler;
    private readonly LootResolutionHandler _lootResolutionHandler;
    private readonly ApplyLootToCharacterInventoryHandler _applyLootHandler;
    private readonly ILootDefinitionProvider _lootDefinitionProvider;
    private readonly RewardSettings _settings;
    private readonly IExperienceDistributor _experienceDistributor;


    public RewardResolutionHandler(
        ApplyResolvedRewardToCharacterHandler progressionHandler,
        LootResolutionHandler lootResolutionHandler,
        ApplyLootToCharacterInventoryHandler applyLootHandler,
        ILootDefinitionProvider lootDefinitionProvider,
        RewardSettings settings)
        : this(progressionHandler, lootResolutionHandler, applyLootHandler, lootDefinitionProvider, settings,
            EvenSplitExperienceDistributor.Instance)
    {
    }


    // PV-XP-010 (ADR-007): a Experience final passa pela Experience Distribution antes de ser aplicada.
    public RewardResolutionHandler(
        ApplyResolvedRewardToCharacterHandler progressionHandler,
        LootResolutionHandler lootResolutionHandler,
        ApplyLootToCharacterInventoryHandler applyLootHandler,
        ILootDefinitionProvider lootDefinitionProvider,
        RewardSettings settings,
        IExperienceDistributor experienceDistributor)
    {
        _experienceDistributor =
            experienceDistributor ?? throw new ArgumentNullException(nameof(experienceDistributor));
        _lootDefinitionProvider =
            lootDefinitionProvider;
        _settings =
            settings;
        _progressionHandler =
            progressionHandler;

        _lootResolutionHandler =
            lootResolutionHandler;

        _applyLootHandler =
            applyLootHandler;
    }


    public async Task<ResolvedReward> Handle(
        CombatResult combatResult,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            combatResult);


        var lootResult =
            _lootResolutionHandler.Handle(
                combatResult);


        await _applyLootHandler.Handle(
            combatResult.CharacterId,
            lootResult,
            cancellationToken);


        // PV-REWARD-010: XP da definição do monstro × XP rate do servidor (DEC-20261106-12).
        // Antes: null (o ataque real nunca entregava XP). Sem vitória ou sem definição → null.
        long? experience = null;

        if (combatResult.Outcome == ProjectVictory.Domain.Combat.CombatOutcome.Victory)
        {
            var definition =
                _lootDefinitionProvider.GetByOpponentId(
                    combatResult.OpponentId);

            if (definition is not null)
            {
                experience =
                    _settings.ExperienceRate.Apply(
                        definition.Experience);
            }
        }

        var reward =
            new ResolvedReward(
                combatResult.CharacterId,
                experience,
                "Combat",
                combatResult.Id);


        // PV-XP-010 (ADR-007/024): a Experience FINAL é repartida pela Experience Distribution e cada
        // parte é aplicada pelo Reward Pipeline (único que muda Experience). Hoje há 1 jogador por
        // Hunt: o único participante é o personagem do combate e recebe o total (mesmo XP de antes).
        if (reward.ExperienceAmount is > 0)
        {
            IReadOnlyList<Guid> participants = [reward.CharacterId];

            foreach (var share in _experienceDistributor.Distribute(reward.ExperienceAmount.Value, participants))
            {
                await _progressionHandler.Handle(
                    new ApplyResolvedRewardToCharacterCommand(
                        share.CharacterId,
                        share.Amount),
                    cancellationToken);
            }
        }
        else
        {
            // Sem Experience (derrota, sem definição ou 0): mesma chamada de antes (não altera nada).
            await _progressionHandler.Handle(
                new ApplyResolvedRewardToCharacterCommand(
                    reward.CharacterId,
                    reward.ExperienceAmount),
                cancellationToken);
        }


        return reward;
    }
}