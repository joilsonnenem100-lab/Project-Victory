using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Character;

public sealed class ValidateEvolutionHandler
{
    private readonly ICharacterRepository _characterRepository;

    public ValidateEvolutionHandler(
        ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task<ValidateEvolutionResult> Handle(
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

        return new ValidateEvolutionResult(
            validation.CharacterId,
            validation.Accepted);
    }
}