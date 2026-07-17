using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Character;

public sealed class CreatePersistentCharacterStateHandler
{
    private readonly ICharacterRepository _characterRepository;

    public CreatePersistentCharacterStateHandler(
        ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task<CreatePersistentCharacterStateResult> Handle(
        CreatePersistentCharacterStateCommand command,
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

        return new CreatePersistentCharacterStateResult(
            character.Id,
            true);
    }
}