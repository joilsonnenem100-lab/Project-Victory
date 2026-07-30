using ProjectVictory.Application.Loot;
using ProjectVictory.Application.Reward;
using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Gameplay;

public sealed class GameplayResultPipeline
{
    private readonly LootResolutionHandler _lootResolutionHandler;
    private readonly ApplyLootToCharacterInventoryHandler _applyLootToCharacterInventoryHandler;
    private readonly RewardResolutionHandler _rewardResolutionHandler;


    public GameplayResultPipeline(
        LootResolutionHandler lootResolutionHandler,
        RewardResolutionHandler rewardResolutionHandler)
    {
        _lootResolutionHandler =
            lootResolutionHandler;

        _rewardResolutionHandler =
            rewardResolutionHandler;

        _applyLootToCharacterInventoryHandler =
            null!;
    }


    public GameplayResultPipeline(
        LootResolutionHandler lootResolutionHandler,
        ApplyLootToCharacterInventoryHandler applyLootToCharacterInventoryHandler,
        RewardResolutionHandler rewardResolutionHandler)
    {
        _lootResolutionHandler =
            lootResolutionHandler;

        _applyLootToCharacterInventoryHandler =
            applyLootToCharacterInventoryHandler;

        _rewardResolutionHandler =
            rewardResolutionHandler;
    }


    public async Task<GameplayResultPipelineResult> Handle(
        CombatResult combatResult,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            combatResult);


        var lootResult =
            _lootResolutionHandler.Handle(
                combatResult);


        if (_applyLootToCharacterInventoryHandler is not null)
        {
            await _applyLootToCharacterInventoryHandler.Handle(
                combatResult.CharacterId,
                lootResult,
                cancellationToken);
        }


        var rewardResult =
            await _rewardResolutionHandler.Handle(
                combatResult,
                cancellationToken);


        return new GameplayResultPipelineResult(
            lootResult,
            rewardResult);
    }
}