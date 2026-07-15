namespace ProjectVictory.Application.Reward;

public interface IRewardProcessor
{
    RewardResult Process(
        RewardRequest request);
}