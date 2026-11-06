using System;

namespace ProjectVictory.Domain.Character;

// PV-CHAR-010/011: Attribute Curve (ADR-029) — Vida e Mana máximas derivadas de Class + Level
// (Level derivado da Experience, ADR-027). Não são armazenadas como valor editável.
//
// PV-CHAR-011 (DEC-20261106-18, referência Tibia): duas fases.
// - Do Level 1 até o VocationLevel: Inicial + floor(ganho sem vocação × (L − 1)).
// - Depois: valor no VocationLevel + floor(ganho da classe × (L − VocationLevel)).
// Com VocationLevel = 1 (padrão) a curva tem uma fase só: Inicial + ganho × (L − 1).
// Os ganhos aceitam fração.
public sealed record AttributeCurve(
    int InitialHealth,
    decimal HealthPerLevel,
    int InitialMana,
    decimal ManaPerLevel,
    int VocationLevel = 1,
    decimal PreVocationHealthPerLevel = 0,
    decimal PreVocationManaPerLevel = 0)
{
    public int MaximumHealthAt(int level) =>
        ValueAt(InitialHealth, PreVocationHealthPerLevel, HealthPerLevel, level);

    public int MaximumManaAt(int level) =>
        ValueAt(InitialMana, PreVocationManaPerLevel, ManaPerLevel, level);

    private int ValueAt(int initial, decimal prePerLevel, decimal perLevel, int level)
    {
        if (level < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }

        if (VocationLevel < 1)
        {
            throw new InvalidOperationException("Vocation level must be at least 1.");
        }

        if (level <= VocationLevel)
        {
            return checked(initial + (int)decimal.Floor(prePerLevel * (level - 1)));
        }

        var atVocationLevel = initial + decimal.Floor(prePerLevel * (VocationLevel - 1));
        var value = atVocationLevel + decimal.Floor(perLevel * (level - VocationLevel));

        // checked: o Level não tem teto (DEC-20261106-19); estouro vira exceção, nunca valor errado.
        return checked((int)value);
    }
}

// PV-CHAR-011: tabela de curvas por classe, em dados (DEC-20261106-18; R-021 resolvido).
//
// Referência Tibia, conferida no código aberto do NabBot (`cogs/utils/tibia.py`, Apache 2.0, com as
// fórmulas da TibiaWiki):
// - sem vocação até o Level 8: Vida = 5 × L + 145 e Mana = 5 × L + 50 (Level 1 = 150/55);
// - do Level 8 em diante, os ganhos da classe (Knight +15/+5, Paladin +10/+15, Druid e Sorcerer
//   +5/+30), conferidos com os ganhos definidos pelo PO.
// - Monk: *** PROVISÓRIO *** — ausente da fonte; mesmo padrão até o 8 e ganhos do PO (+12/+15).
// Os valores por classe ficam em CharacterClassDefinition; a fase sem vocação, aqui.
public static class AttributeCurveTable
{
    public const int VocationLevel = 8;

    public const decimal PreVocationHealthPerLevel = 5;

    public const decimal PreVocationManaPerLevel = 5;

    public static AttributeCurve ForClass(
        CharacterClass characterClass,
        int ascension = 1)
    {
        if (ascension < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(ascension));
        }

        // A Ascension é derivada do Level (DEC-20261106-19) e o Level nunca volta a 1, então a curva
        // não muda com a Ascension (R-022 sem efeito).
        var definition =
            CharacterClassDefinition.Get(characterClass);

        return new AttributeCurve(
            definition.InitialHealth,
            definition.HealthGrowth,
            definition.InitialMana,
            definition.ManaGrowth,
            VocationLevel,
            PreVocationHealthPerLevel,
            PreVocationManaPerLevel);
    }
}
