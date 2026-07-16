using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Character;

public sealed class ApplyEvolutionHandler
{
    private readonly ICharacterRepository _characterRepository;

    public ApplyEvolutionHandler(
        ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task<ApplyEvolutionResult> Handle(
        CreateEvolutionCommand command,
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

        var validation = character.ValidateEvolution();

        if (!validation.Accepted)
        {
            return new ApplyEvolutionResult(
                character.Id,
                false);
        }

        character.ApplyEvolution();

        await _characterRepository.UpdateAsync(
            character,
            cancellationToken);

        return new ApplyEvolutionResult(
            character.Id,
            true);
    }
}