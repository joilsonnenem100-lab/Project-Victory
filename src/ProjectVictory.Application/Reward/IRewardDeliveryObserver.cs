using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Loot;

namespace ProjectVictory.Application.Reward;

// PV-HUD-ANALYZER-010: observador SÓ DE LEITURA do loot entregue pelo Reward Pipeline (ADR-024).
// O Reward não conhece quem observa (ex.: Hunt Analyzer); o observador não altera o Reward nem o
// personagem (Aggregate Ownership).
public interface IRewardDeliveryObserver
{
    void OnLootDelivered(CombatResult combatResult, LootResolutionResult loot);
}
