using System;

namespace ProjectVictory.Domain.Character;

// PV-CHAR-010: Attribute Curve (ADR-029) — Vida e Mana máximas derivadas de Class + Level
// (Level derivado de Experience + Ascension, ADR-027). Não são armazenadas como valor editável.
//
// Máximo no Level L = Inicial + floor(Ganho por Level × (L − 1)).
// O ganho aceita fração (ex.: +0.75), porque knowledge-base/progression.md usa valores fracionários.
public sealed record AttributeCurve(
    int InitialHealth,
    decimal HealthPerLevel,
    int InitialMana,
    decimal ManaPerLevel)
{
    public int MaximumHealthAt(int level) =>
        ValueAt(InitialHealth, HealthPerLevel, level);

    public int MaximumManaAt(int level) =>
        ValueAt(InitialMana, ManaPerLevel, level);

    private static int ValueAt(int initial, decimal perLevel, int level)
    {
        if (level < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }

        return checked(initial + (int)decimal.Floor(perLevel * (level - 1)));
    }
}

// PV-CHAR-010: tabela de curvas por classe.
//
// *** PROVISÓRIO *** — os números vêm de CharacterClassDefinition (o que o jogo já usa desde o
// Vertical Slice), para não mudar o personagem de Level 1. Eles CONFLITAM com
// knowledge-base/progression.md ("HP inicial 100 para todas; Knight +0.75 por Level…") — pergunta
// ao PO registrada como R-021. Trocar os valores = trocar só esta tabela (dados), sem mudar regra.
// Ascension: a curva usa só o Level, que volta a 1 na Ascension; isso CONTRADIZ "atributos
// permanentes são mantidos" (progression.md / ADR-026). Sem efeito hoje (Ascension fixa em 1);
// a regra fica para o contrato de Ascension (R-022).
public static class AttributeCurveTable
{
    public static AttributeCurve ForClass(
        CharacterClass characterClass,
        int ascension = 1)
    {
        if (ascension < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(ascension));
        }

        var definition =
            CharacterClassDefinition.Get(characterClass);

        return new AttributeCurve(
            definition.InitialHealth,
            definition.HealthGrowth,
            definition.InitialMana,
            definition.ManaGrowth);
    }
}
