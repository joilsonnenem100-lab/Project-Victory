using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectVictory.Domain.Character;

// PV-CHAR-012 (DEC-20261106-26): as skills do personagem são SÓ as do Tibia (o Level fica à parte,
// derivado da Experience). Status: HP e Mana. Não há Capacidade nem atributos inventados
// (Força, Destreza, Inteligência, Vitalidade, Sorte, Attack Speed — DEC-20261106-27).
public enum TibiaSkill
{
    MagicLevel,
    FistFighting,
    ClubFighting,
    SwordFighting,
    AxeFighting,
    DistanceFighting,
    Shielding,
    Fishing
}

// PV-CHAR-012 (DEC-20261106-29): tema elemental da classe — SÓ DESCRITIVO. Não há mecânica
// elemental (resistência, fraqueza, tipo de dano); o dano em combate continua Physical (PV-COMBAT-013).
public enum ElementTheme
{
    Physical,
    Holy,
    Earth,
    Ice,
    Energy,
    Fire
}

// Identidade da classe (dados): descrição, skills principais do Tibia e tema elemental.
public sealed record ClassIdentity(
    CharacterClass Class,
    string Description,
    IReadOnlyList<TibiaSkill> MainSkills,
    IReadOnlyList<ElementTheme> Elements);

public static class ClassIdentityTable
{
    public static IReadOnlyList<ClassIdentity> All { get; } =
    [
        // Knight: como já decidido (DEC-20261106-23) — combate corpo a corpo, ataque básico forte.
        new(CharacterClass.Knight,
            "Guerreiro de linha de frente: espada, machado ou clava, com escudo.",
            [TibiaSkill.SwordFighting, TibiaSkill.AxeFighting, TibiaSkill.ClubFighting, TibiaSkill.Shielding],
            [ElementTheme.Physical]),

        // Paladin: como já decidido (DEC-20261106-23) — ataque à distância muito forte; magias Holy
        // e de elementos ficam para depois (por isso o Holy aparece só como tema).
        new(CharacterClass.Paladin,
            "Atirador: arcos, bestas e lanças; magia sagrada no futuro.",
            [TibiaSkill.DistanceFighting, TibiaSkill.Shielding],
            [ElementTheme.Physical, ElementTheme.Holy]),

        // Sorcerer (DEC-20261106-29): energia e fogo.
        new(CharacterClass.Sorcerer,
            "Mago de energia e fogo.",
            [TibiaSkill.MagicLevel],
            [ElementTheme.Energy, ElementTheme.Fire]),

        // Druid (DEC-20261106-29): mago da natureza; terra e gelo.
        new(CharacterClass.Druid,
            "Mago da natureza: terra e gelo.",
            [TibiaSkill.MagicLevel],
            [ElementTheme.Earth, ElementTheme.Ice]),

        // Monk (DEC-20261106-29): lutador de Fist Fighting, com armas de luva e afins.
        new(CharacterClass.Monk,
            "Lutador de Fist Fighting, com luvas e armas afins.",
            [TibiaSkill.FistFighting],
            [ElementTheme.Physical])
    ];

    public static ClassIdentity For(CharacterClass characterClass) =>
        All.SingleOrDefault(identity => identity.Class == characterClass)
        ?? throw new ArgumentOutOfRangeException(nameof(characterClass));
}
