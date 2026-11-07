using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Character;

public sealed record CreateCharacterCommand(
    string Name,
    CharacterClass CharacterClass,
    // PV-CHAR-013 (PO-37, aditivo e opcional): gênero e aparência; sem valor, os padrões (PROVISÓRIOS).
    CharacterGender? Gender = null,
    string? Outfit = null);