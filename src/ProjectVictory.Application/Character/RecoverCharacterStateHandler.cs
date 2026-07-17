using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Character;

public sealed class RecoverCharacterStateHandler
{
    private readonly ICharacterRepository _characterRepository;

    public RecoverCharacterStateHandler(
        ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task<RecoverCharacterStateResult> Handle(
        RecoverCharacterStateCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var character = await _characterRepository.GetByIdAsync(
            command.CharacterId,
            cancellationToken);

        if (character is null)
        {
            return new RecoverCharacterStateResult(
                command.CharacterId,
                false);
        }

        return new RecoverCharacterStateResult(
            character.Id,
            true);
    }
}