using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Reward;

public sealed class ApplyRewardHandler
{
    private readonly IRewardProcessor _rewardProcessor;
    private readonly ICharacterRepository _characterRepository;

    public ApplyRewardHandler(
        IRewardProcessor rewardProcessor,
        ICharacterRepository characterRepository)
    {
        _rewardProcessor = rewardProcessor;
        _characterRepository = characterRepository;
    }

    public async Task<ApplyRewardResult> Handle(
        ApplyRewardCommand command,
        CancellationToken cancellationToken = default)
    {
        var character =
            await _characterRepository.GetByIdAsync(
                command.CharacterId,
                cancellationToken);

        if (character is null)
        {
            throw new InvalidOperationException(
                "Character not found.");
        }

        var reward =
            _rewardProcessor.Process(
                command.Request);

        if (reward.Experience > 0)
        {
            character.GainExperience(
                new ProjectVictory.Domain.ValueObjects.Experience(
                    reward.Experience));
        }

        await _characterRepository.UpdateAsync(
            character,
            cancellationToken);

        return new ApplyRewardResult(
            character.Id,
            true);
    }
}
