using System;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.Combat.Rules;

// PV-COMBAT-012: regra de dano atrás de interface. A fórmula final (Tibia) entra depois,
// trocando só a implementação registrada.
public interface IDamageFormula
{
    int RollDamage(
        Combatant attacker,
        Combatant defender,
        CombatActionKind kind,
        IRandomSource random);

    // PV-COMBAT-013: dano com multiplicador explícito (skill de classe).
    int RollDamage(
        Combatant attacker,
        Combatant defender,
        decimal multiplier,
        IRandomSource random);
}

// Constantes da fórmula provisória, como dados.
public sealed record DamageFormulaSettings(
    decimal SkillAttackFactor,
    int LevelDivisor,
    int LevelDefenseDivisor)
{
    // *** PROVISÓRIO *** — inspirado no corpo-a-corpo clássico do Tibia:
    // dano máximo = 0,085 × skill × ataque da arma + Level / 5.
    public static DamageFormulaSettings Provisional { get; } = new(0.085m, 5, 10);
}

// PV-COMBAT-012: *** PROVISÓRIO *** — modelo inspirado no Tibia, a partir de
// knowledge-base/ascension.md (conteúdo de Combat): o dano depende de Level, skill e arma;
// a defesa depende de Level, equipamento (armadura) e shield. As fórmulas oficiais estão
// pendentes nesse mesmo documento ("Fórmula de dano", "Fórmula de defesa").
//
// 1. Dano máximo = floor(fator × AttackSkill × WeaponAttack + Level / LevelDivisor),
//    multiplicado por AbilityDamageMultiplier na ClassAbility.
// 2. Dano bruto = sorteio uniforme em [0, máximo].
// 3. Armadura: reduz um sorteio em [floor(Armor / 2), Armor − 1] (Armor > 1), como no Tibia.
// 4. Shield/defesa: reduz um sorteio em [0, floor((Defense + Level / LevelDefenseDivisor) / 2)].
// 5. Dano final = max(0, bruto − armadura − defesa).
public sealed class ProvisionalTibiaLikeDamageFormula : IDamageFormula
{
    private readonly DamageFormulaSettings _settings;

    public ProvisionalTibiaLikeDamageFormula()
        : this(DamageFormulaSettings.Provisional)
    {
    }

    public ProvisionalTibiaLikeDamageFormula(DamageFormulaSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public int MaximumDamage(Combatant attacker, CombatActionKind kind)
    {
        ArgumentNullException.ThrowIfNull(attacker);

        var multiplier = kind switch
        {
            CombatActionKind.ClassAbility => attacker.Profile.AbilityDamageMultiplier,
            CombatActionKind.BasicAttack => attacker.Profile.BasicAttackMultiplier,
            _ => 1m
        };

        return MaximumDamage(attacker, multiplier);
    }

    // PV-COMBAT-013: dano máximo com multiplicador explícito (skill ou ataque básico).
    public int MaximumDamage(Combatant attacker, decimal multiplier)
    {
        ArgumentNullException.ThrowIfNull(attacker);

        var profile = attacker.Profile;
        var baseMax =
            ((_settings.SkillAttackFactor * profile.AttackSkill * profile.WeaponAttack)
            + ((decimal)attacker.Level / _settings.LevelDivisor)) * multiplier;

        return Math.Max(0, (int)decimal.Floor(baseMax));
    }

    public int RollDamage(
        Combatant attacker,
        Combatant defender,
        CombatActionKind kind,
        IRandomSource random)
    {
        ArgumentNullException.ThrowIfNull(attacker);

        return Roll(defender, MaximumDamage(attacker, kind), random);
    }

    public int RollDamage(
        Combatant attacker,
        Combatant defender,
        decimal multiplier,
        IRandomSource random)
    {
        ArgumentNullException.ThrowIfNull(attacker);

        return Roll(defender, MaximumDamage(attacker, multiplier), random);
    }

    private int Roll(
        Combatant defender,
        int maximumDamage,
        IRandomSource random)
    {
        ArgumentNullException.ThrowIfNull(defender);
        ArgumentNullException.ThrowIfNull(random);

        var raw = random.NextInclusive(0, maximumDamage);

        var armor = defender.Profile.Armor;
        var armorReduction = armor > 1
            ? random.NextInclusive(armor / 2, armor - 1)
            : 0;

        var defense = defender.Profile.Defense + (defender.Level / _settings.LevelDefenseDivisor);
        var defenseReduction = random.NextInclusive(0, Math.Max(0, defense / 2));

        return Math.Max(0, raw - armorReduction - defenseReduction);
    }
}
