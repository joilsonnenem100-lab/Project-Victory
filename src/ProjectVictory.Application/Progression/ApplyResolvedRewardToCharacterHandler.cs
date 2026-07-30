using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Progression;

public sealed class ApplyResolvedRewardToCharacterHandler
{
    private readonly ICharacterRepository _characterRepository;


    public ApplyResolvedRewardToCharacterHandler(
        ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }


    public async Task<ApplyResolvedRewardToCharacterResult> Handle(
        ApplyResolvedRewardToCharacterCommand command,
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


        if (!command.ExperienceAmount.HasValue)
        {
            return new ApplyResolvedRewardToCharacterResult(
                character.Id,
                0,
                false);
        }


        if (command.ExperienceAmount.Value <= 0)
        {
            return new ApplyResolvedRewardToCharacterResult(
                character.Id,
                0,
                false);
        }


        character.GainExperience(
            new ProjectVictory.Domain.ValueObjects.Experience(
                command.ExperienceAmount.Value));


        await _characterRepository.UpdateAsync(
            character,
            cancellationToken);


        return new ApplyResolvedRewardToCharacterResult(
            character.Id,
            command.ExperienceAmount.Value,
            true);
    }
}