using ProjectVictory.Domain.Character;
using CharacterEntity = ProjectVictory.Domain.Entities.Character;

namespace ProjectVictory.Application.Reward;

public sealed class ConfirmRewardStateHandler
{
    private readonly ICharacterRepository _characterRepository;

    public ConfirmRewardStateHandler(
        ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task<ConfirmRewardStateResult> Handle(
        ConfirmRewardStateCommand command,
        CancellationToken cancellationToken = default)
    {
        var character = await _characterRepository.GetByIdAsync(
            command.CharacterId,
            cancellationToken);

        if (character is null)
        {
            throw new InvalidOperationException(
                "Character not found.");
        }

        ValidateCharacterState(character);

        return new ConfirmRewardStateResult(
            character.Id,
            true);
    }

    private static void ValidateCharacterState(
        CharacterEntity character)
    {
        ArgumentNullException.ThrowIfNull(character);

        if (character.Experience is null)
        {
            throw new InvalidOperationException(
                "Character experience state is invalid.");
        }

        if (character.Level is null)
        {
            throw new InvalidOperationException(
                "Character level state is invalid.");
        }
    }
}