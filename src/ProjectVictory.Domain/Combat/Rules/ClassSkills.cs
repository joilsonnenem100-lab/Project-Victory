using System;
using System.Collections.Generic;
using System.Linq;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Domain.Combat.Rules;

// PV-COMBAT-013 (DEC-20261106-16): tipo de dano extensível. Hoje só existe o físico; danos e
// resistências elementais (fogo, gelo, energia, terra, holy…) entram depois, como novos valores.
public enum DamageType
{
    Physical
}

// Alvo de uma skill.
public enum SkillTargeting
{
    SingleTarget,
    Area
}

// Atributo que escala o dano da skill (referência Tibia: arma/skill de combate, Magic Level, Fist…).
public enum SkillScaling
{
    Weapon,
    Distance,
    MagicLevel,
    FistFighting
}

// PV-COMBAT-013: uma skill de classe, como dado. Nomes e números PROVISÓRIOS (DEC-20261106-23).
public sealed record SkillDefinition(
    string Id,
    string Name,
    CharacterClass Class,
    SkillTargeting Targeting,
    int ManaCost,
    decimal DamageMultiplier,
    int UnlockLevel,
    SkillScaling Scaling,
    DamageType DamageType = DamageType.Physical);

// PV-COMBAT-013: skills e força do ataque básico por classe, em DADOS.
//
// Regras do PO (DEC-20261106-23):
// - Knight: 2 em área de dano BAIXO + 2 de alvo único de dano MÉDIO; ataque básico FORTE.
// - Paladin: 2 em área + 2 de alvo único, dano MÉDIO equilibrado; ataque básico à distância de alvo
//   único MUITO FORTE (Holy e elementos depois).
// - Sorcerer: 2 em área + 2 de alvo único, dano ALTO; o Magic Level é o contador do dano final.
// - Druid e Monk: sem regras do PO — PROVISÓRIO: 2 em área + 2 de alvo único, dano médio; Druid
//   escala por Magic Level, Monk por Fist Fighting.
//
// *** PROVISÓRIO *** — TODOS os nomes (estilo Tibia, nomes genéricos/originais, DEC-20261106-15),
// custos, multiplicadores e Levels de desbloqueio são balanceamento do Arquiteto para o PO ajustar.
// Trocar = trocar esta tabela. Escala do multiplicador: 1,0 = dano do ataque básico padrão.
public static class ClassSkillTable
{
    public static IReadOnlyList<SkillDefinition> All { get; } =
    [
        // Knight — área baixa, alvo único média; escala pela arma (Sword).
        new("knight-whirl-strike", "Whirl Strike", CharacterClass.Knight, SkillTargeting.Area, 10, 0.8m, 1, SkillScaling.Weapon),
        new("knight-ground-shaker", "Ground Shaker", CharacterClass.Knight, SkillTargeting.Area, 20, 1.0m, 15, SkillScaling.Weapon),
        new("knight-brutal-blow", "Brutal Blow", CharacterClass.Knight, SkillTargeting.SingleTarget, 15, 1.4m, 5, SkillScaling.Weapon),
        new("knight-fierce-thrust", "Fierce Thrust", CharacterClass.Knight, SkillTargeting.SingleTarget, 25, 1.7m, 20, SkillScaling.Weapon),

        // Paladin — dano médio equilibrado; escala por Distance.
        new("paladin-arrow-rain", "Arrow Rain", CharacterClass.Paladin, SkillTargeting.Area, 15, 1.2m, 1, SkillScaling.Distance),
        new("paladin-piercing-volley", "Piercing Volley", CharacterClass.Paladin, SkillTargeting.Area, 25, 1.4m, 15, SkillScaling.Distance),
        new("paladin-aimed-shot", "Aimed Shot", CharacterClass.Paladin, SkillTargeting.SingleTarget, 15, 1.3m, 5, SkillScaling.Distance),
        new("paladin-radiant-shot", "Radiant Shot", CharacterClass.Paladin, SkillTargeting.SingleTarget, 25, 1.5m, 20, SkillScaling.Distance),

        // Sorcerer — dano alto; Magic Level conta o dano final.
        new("sorcerer-flame-wave", "Flame Wave", CharacterClass.Sorcerer, SkillTargeting.Area, 20, 1.6m, 1, SkillScaling.MagicLevel),
        new("sorcerer-fire-storm", "Fire Storm", CharacterClass.Sorcerer, SkillTargeting.Area, 35, 2.0m, 15, SkillScaling.MagicLevel),
        new("sorcerer-spark-bolt", "Spark Bolt", CharacterClass.Sorcerer, SkillTargeting.SingleTarget, 15, 1.9m, 5, SkillScaling.MagicLevel),
        new("sorcerer-arcane-lance", "Arcane Lance", CharacterClass.Sorcerer, SkillTargeting.SingleTarget, 30, 2.4m, 20, SkillScaling.MagicLevel),

        // Druid — PROVISÓRIO (sem regra do PO): dano médio; Magic Level.
        new("druid-frost-wave", "Frost Wave", CharacterClass.Druid, SkillTargeting.Area, 20, 1.2m, 1, SkillScaling.MagicLevel),
        new("druid-stone-rain", "Stone Rain", CharacterClass.Druid, SkillTargeting.Area, 35, 1.5m, 15, SkillScaling.MagicLevel),
        new("druid-frost-shard", "Frost Shard", CharacterClass.Druid, SkillTargeting.SingleTarget, 15, 1.4m, 5, SkillScaling.MagicLevel),
        new("druid-thorn-lance", "Thorn Lance", CharacterClass.Druid, SkillTargeting.SingleTarget, 30, 1.7m, 20, SkillScaling.MagicLevel),

        // Monk — PROVISÓRIO (sem regra do PO): dano médio; Fist Fighting.
        new("monk-sweeping-kick", "Sweeping Kick", CharacterClass.Monk, SkillTargeting.Area, 15, 1.1m, 1, SkillScaling.FistFighting),
        new("monk-whirlwind-palm", "Whirlwind Palm", CharacterClass.Monk, SkillTargeting.Area, 25, 1.3m, 15, SkillScaling.FistFighting),
        new("monk-power-punch", "Power Punch", CharacterClass.Monk, SkillTargeting.SingleTarget, 15, 1.4m, 5, SkillScaling.FistFighting),
        new("monk-chi-strike", "Chi Strike", CharacterClass.Monk, SkillTargeting.SingleTarget, 25, 1.7m, 20, SkillScaling.FistFighting)
    ];

    // Força do ataque básico por classe (multiplicador sobre a fórmula). PROVISÓRIO.
    public static decimal BasicAttackMultiplier(CharacterClass characterClass) => characterClass switch
    {
        CharacterClass.Knight => 1.3m,   // "ataque básico forte"
        CharacterClass.Paladin => 1.6m,  // "ataque básico à distância de alvo único muito forte"
        CharacterClass.Sorcerer => 1.0m,
        CharacterClass.Druid => 1.0m,
        CharacterClass.Monk => 1.1m,
        _ => throw new ArgumentOutOfRangeException(nameof(characterClass))
    };

    public static IReadOnlyList<SkillDefinition> ForClass(CharacterClass characterClass) =>
        All.Where(s => s.Class == characterClass).ToList();

    // Skills já desbloqueadas pelo Level do personagem.
    public static IReadOnlyList<SkillDefinition> UnlockedFor(CharacterClass characterClass, int level) =>
        All.Where(s => s.Class == characterClass && s.UnlockLevel <= level).ToList();
}

// PV-COMBAT-013: política automática de escolha da skill (Secondary Action do jogador), em DADOS.
// *** PROVISÓRIO *** — com MinimumEnemiesForArea ou mais inimigos vivos, usa a melhor skill em área
// desbloqueada que caiba na mana; com menos, a melhor de alvo único; se não houver skill do tipo
// certo desbloqueada e com mana, nenhuma (o turno fica só com o ataque básico). "Melhor" = maior
// multiplicador. Trocar = trocar estes dados ou a política.
public sealed record SkillSelectionPolicy(int MinimumEnemiesForArea = 2)
{
    public static SkillSelectionPolicy Default { get; } = new();

    public SkillDefinition? Choose(
        IReadOnlyList<SkillDefinition> unlocked,
        int aliveEnemies,
        int currentMana)
    {
        ArgumentNullException.ThrowIfNull(unlocked);

        var affordable = unlocked.Where(s => s.ManaCost <= currentMana).ToList();

        if (aliveEnemies <= 0 || affordable.Count == 0)
        {
            return null;
        }

        var preferred = aliveEnemies >= MinimumEnemiesForArea ? SkillTargeting.Area : SkillTargeting.SingleTarget;

        return affordable
            .Where(s => s.Targeting == preferred)
            .OrderByDescending(s => s.DamageMultiplier)
            .FirstOrDefault();
    }
}
