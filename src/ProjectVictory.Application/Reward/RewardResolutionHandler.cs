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


    public RewardResolutionHandler(
        ApplyResolvedRewardToCharacterHandler progressionHandler,
        LootResolutionHandler lootResolutionHandler,
        ApplyLootToCharacterInventoryHandler applyLootHandler,
        ILootDefinitionProvider lootDefinitionProvider,
        RewardSettings settings)
    {
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


        await _progressionHandler.Handle(
            new ApplyResolvedRewardToCharacterCommand(
                reward.CharacterId,
                reward.ExperienceAmount),
            cancellationToken);


        return reward;
    }
}