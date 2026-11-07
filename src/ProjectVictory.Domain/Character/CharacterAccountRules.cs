using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectVictory.Domain.Character;

// PV-CHAR-013 (PO-36, PO-57; R-066): no máximo 5 personagens por conta, UM por classe; 3 slots
// utilizáveis e 2 bloqueados (destravados com gemas no futuro — ainda sem mecanismo de destrave).
public static class CharacterSlotRules
{
    public const int MaximumSlots = 5;

    public const int UnlockedSlots = 3;
}

public enum CharacterSlotState
{
    Free = 1,
    Occupied = 2,
    Locked = 3
}

// PV-CHAR-013 (PO-38: "formato do Tibia"). Regras confirmadas em fonte pública (TibiaQA, reforma das
// regras de nome de 2008): maiúscula SÓ no início de cada palavra; palavra de uma letra só é proibida.
// *** PROVISÓRIO *** (sem fonte oficial encontrada nesta fila): só letras A–Z e espaço simples; de 2 a 29
// caracteres; no máximo 3 palavras; nome único sem diferenciar maiúsculas. Trocar aqui.
public static class CharacterNameRules
{
    public const int MinimumLength = 2;

    public const int MaximumLength = 29;

    public const int MaximumWords = 3;

    // null = válido; senão, o motivo (mensagem para o 400).
    public static string? Validate(string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return "Character name is required.";
        }

        if (name.Length < MinimumLength || name.Length > MaximumLength)
        {
            return $"Character name must have {MinimumLength} to {MaximumLength} characters.";
        }

        if (name.Any(c => !(c is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or ' ')))
        {
            return "Character name may contain only letters (A-Z) and single spaces.";
        }

        var words = name.Split(' ');

        if (words.Any(word => word.Length == 0))
        {
            return "Character name cannot start or end with a space or have double spaces.";
        }

        if (words.Length > MaximumWords)
        {
            return $"Character name may have at most {MaximumWords} words.";
        }

        if (words.Any(word => word.Length < 2))
        {
            return "Each word of the character name must have at least 2 letters.";
        }

        if (words.Any(word => !char.IsUpper(word[0]) || word.Skip(1).Any(char.IsUpper)))
        {
            return "Each word must start with a capital letter followed by lowercase letters.";
        }

        return null;
    }
}

// PV-CHAR-013 (PO-37: gênero e aparência). *** PROVISÓRIO *** — opções de aparência (nomes genéricos,
// DEC-20261106-15) até existir a arte; trocar aqui.
public enum CharacterGender
{
    Unspecified = 0,
    Male = 1,
    Female = 2
}

public static class CharacterAppearanceOptions
{
    public const string DefaultOutfit = "Traveler";

    public static IReadOnlyList<string> Outfits { get; } = ["Traveler", "Scout", "Scholar", "Guard"];

    public static IReadOnlyList<CharacterGender> Genders { get; } = [CharacterGender.Male, CharacterGender.Female];

    public static string? FindOutfit(string? outfit) =>
        Outfits.FirstOrDefault(o => string.Equals(o, outfit, StringComparison.OrdinalIgnoreCase));
}

// PV-CHAR-013 (PO-39: exclusão com espera de 7 dias). A exclusão fica PENDENTE com data e é finalizada
// pelo servidor quando a data passa. *** PROVISÓRIO ***: o cancelamento durante a espera (estilo Tibia).
public static class CharacterDeletionRules
{
    public static TimeSpan Wait { get; } = TimeSpan.FromDays(7);

    public static DateTimeOffset ScheduledFor(DateTimeOffset requestedAt) => requestedAt + Wait;

    public static bool IsDue(DateTimeOffset scheduledFor, DateTimeOffset now) => now >= scheduledFor;
}
