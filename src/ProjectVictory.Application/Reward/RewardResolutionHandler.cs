using ProjectVictory.Domain.Combat;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Loot;

namespace ProjectVictory.Application.Reward;

public sealed class RewardResolutionHandler
{
    private readonly ApplyResolvedRewardToCharacterHandler _progressionHandler;
    private readonly LootResolutionHandler _lootResolutionHandler;
    private readonly ApplyLootToCharacterInventoryHandler _applyLootHandler;


    public RewardResolutionHandler(
        ApplyResolvedRewardToCharacterHandler progressionHandler,
        LootResolutionHandler lootResolutionHandler,
        ApplyLootToCharacterInventoryHandler applyLootHandler)
    {
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


        var reward =
            new ResolvedReward(
                combatResult.CharacterId,
                null,
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