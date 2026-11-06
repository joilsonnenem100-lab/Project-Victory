using ProjectVictory.Application.Combat;

namespace ProjectVictory.Infrastructure.Combat;

// *** PROVISÓRIO *** (DEC-20261106-02): valores de cura das poções sem decisão do PO.
// Trocar = trocar estes dados (ou a implementação registrada de ISupplyEffects).
public sealed class ProvisionalSupplyEffects : ISupplyEffects
{
    public int HealthPotionHealing => 75;

    public int ManaPotionRestore => 50;
}
