using ProjectVictory.Application.Combat;

namespace ProjectVictory.Infrastructure.Combat;

// *** PROVISÓRIO *** (DEC-20261106-02): valores de cura das poções sem decisão do PO.
// Trocar = trocar estes dados (ou a implementação registrada de ISupplyEffects).
public sealed class ProvisionalSupplyEffects : ISupplyEffects
{
    // PV-CONTENT-012: os valores agora vêm do PotionCatalog (nível pequeno; os mesmos 75/50 de antes).
    public int HealthPotionHealing => ProjectVictory.Application.Supply.PotionCatalog.SmallHealthAmount;

    public int ManaPotionRestore => ProjectVictory.Application.Supply.PotionCatalog.SmallManaAmount;
}
