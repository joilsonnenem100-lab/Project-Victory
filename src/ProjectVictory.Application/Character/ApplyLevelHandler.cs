using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Character;

public sealed class ApplyLevelHandler
{
    private readonly ICharacterRepository _characterRepository;

    public ApplyLevelHandler(
        ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task<ApplyLevelResult> Handle(
        ForwardLevelCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var character = await _characterRepository.GetByIdAsync(
            command.CharacterId,
            cancellationToken);

        if (character is null)
        {
            throw new InvalidOperationException(
                "Character not found.");
        }

        var validation = character.ValidateLevel(
            command.Level);

        if (!validation.Accepted)
        {
            return new ApplyLevelResult(
                character.Id,
                false);
        }

        character.ApplyLevel(
            command.Level);

        await _characterRepository.UpdateAsync(
            character,
            cancellationToken);

        return new ApplyLevelResult(
            character.Id,
            true);
    }
}