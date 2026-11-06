namespace ProjectVictory.Application.Combat;

// PV-COMBAT-012 / PV-SUPPLY-010: efeito das poções, como dados (usado pelo ICombatResolver).
public interface ISupplyEffects
{
    int HealthPotionHealing { get; }

    int ManaPotionRestore { get; }
}
