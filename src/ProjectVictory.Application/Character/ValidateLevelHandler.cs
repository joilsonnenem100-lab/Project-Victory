using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Character;

public sealed class ValidateLevelHandler
{
    private readonly ICharacterRepository _characterRepository;

    public ValidateLevelHandler(
        ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task<ValidateLevelResult> Handle(
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

        return new ValidateLevelResult(
            validation.CharacterId,
            validation.Accepted);
    }
}