using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Character;

public sealed record CreateCharacterCommand(
    string Name,
    CharacterClass CharacterClass);