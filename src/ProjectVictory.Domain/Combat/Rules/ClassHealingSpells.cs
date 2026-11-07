using System;
using System.Collections.Generic;
using System.Linq;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Domain.Combat.Rules;

// PV-COMBAT-014 (PO-53): faixa de Level da spell de cura. A partir de FromLevel, a cura vale
// PercentOfMaximumHealth % da vida MÁXIMA (hipótese de trabalho registrada no contrato).
public sealed record HealingBand(int FromLevel, int PercentOfMaximumHealth);

// PV-COMBAT-014 (PO-53): a spell de cura de uma vocação, como DADO. A porcentagem melhora com o Level
// por faixas (sem um número por Level). Cada uso gasta ManaCost de Mana.
public sealed record HealingSpellDefinition(
    string Id,
    string Name,
    CharacterClass Class,
    int ManaCost,
    IReadOnlyList<HealingBand> Bands)
{
    // Porcentagem da faixa mais alta cujo FromLevel <= level (0 se nenhuma faixa vale).
    public int PercentForLevel(int level) =>
        Bands.Where(b => b.FromLevel <= level)
            .OrderByDescending(b => b.FromLevel)
            .Select(b => b.PercentOfMaximumHealth)
            .FirstOrDefault();

    // Cura bruta: % da vida máxima (arredondada para baixo), no mínimo 1 quando a % é positiva.
    // O limite pelo máximo de vida (só cura o que falta) é aplicado pelo resolver.
    public int RawHealingFor(int level, int maximumHealth)
    {
        var percent = PercentForLevel(level);

        if (percent <= 0 || maximumHealth <= 0)
        {
            return 0;
        }

        return Math.Max(1, (int)((long)maximumHealth * percent / 100));
    }
}

// PV-COMBAT-014 (PO-53, PO-01): 1 spell de cura por vocação, em DADOS.
//
// *** PROVISÓRIO *** — nomes (genéricos, estilo Tibia, DEC-20261106-15), custo de Mana e faixas de
// Level/porcentagem são balanceamento do Arquiteto para o PO ajustar. Trocar = trocar esta tabela.
// - Todas as vocações: 10% (Level 1), 15% (Level 20), 20% (Level 50), 25% (Level 100); 20 de Mana.
// - ~~Druid (PO-01: "dano elemental e cura própria"): +5 pontos em cada faixa.~~ PV-COMBAT-015 (PO-59,
//   DEC-20261107-05): a cura é % da vida MÁXIMA, IGUAL para todas as vocações, Druid incluído.
public static class ClassHealingSpellTable
{
    public const int DefaultManaCost = 20;

    public static IReadOnlyList<HealingBand> StandardBands { get; } =
    [
        new(1, 10),
        new(20, 15),
        new(50, 20),
        new(100, 25)
    ];

    public static IReadOnlyList<HealingSpellDefinition> All { get; } =
    [
        new("knight-wound-mending", "Wound Mending", CharacterClass.Knight, DefaultManaCost, StandardBands),
        new("paladin-divine-mending", "Divine Mending", CharacterClass.Paladin, DefaultManaCost, StandardBands),
        new("sorcerer-arcane-mending", "Arcane Mending", CharacterClass.Sorcerer, DefaultManaCost, StandardBands),
        new("druid-nature-mending", "Nature Mending", CharacterClass.Druid, DefaultManaCost, StandardBands),
        new("monk-inner-mending", "Inner Mending", CharacterClass.Monk, DefaultManaCost, StandardBands)
    ];

    public static HealingSpellDefinition? ForClass(CharacterClass characterClass) =>
        All.FirstOrDefault(s => s.Class == characterClass);
}

// PV-COMBAT-014: quando o seletor automático usa a spell de cura, em DADOS/configuração.
// *** PROVISÓRIO *** — "vida abaixo de BelowPercent % da máxima" (padrão 50); null = nunca usar
// (desliga a cura por spell). Configurável por "Combat:HealingSpellBelowPercent" (Program.cs).
public sealed record HealingSpellPolicy(int? BelowPercent = HealingSpellPolicy.DefaultBelowPercent)
{
    public const string BelowPercentKey = "Combat:HealingSpellBelowPercent";

    public const int DefaultBelowPercent = 50;

    public static HealingSpellPolicy Default { get; } = new();

    public static HealingSpellPolicy Disabled { get; } = new((int?)null);

    // Vida abaixo do limite (comparação inteira, mesmo padrão da SupplyPolicy).
    public bool WantsHealing(int currentHealth, int maximumHealth) =>
        BelowPercent is { } percent
        && maximumHealth > 0
        && (long)currentHealth * 100 < (long)percent * maximumHealth;
}
