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

// PV-HUD-ANALYZER-010 (ressalva R1): destino das falhas de observadores. A falha de um observador é
// isolada (não derruba a entrega da recompensa) e vai para cá; a composição registra no log do host.
// A Application não conhece log (mesmo padrão do IEventFailureSink, ADR-058).
public interface IRewardObserverFailureSink
{
    void Report(Type observer, Guid characterId, Exception error);
}

// Padrão em memória (testes e composição sem host): guarda as últimas falhas.
public sealed class RecordingRewardObserverFailureSink : IRewardObserverFailureSink
{
    public const int Maximum = 64;

    private readonly System.Collections.Concurrent.ConcurrentQueue<(Type Observer, Guid CharacterId, Exception Error)> _failures = new();

    public IReadOnlyList<(Type Observer, Guid CharacterId, Exception Error)> Failures => _failures.ToList();

    public void Report(Type observer, Guid characterId, Exception error)
    {
        _failures.Enqueue((observer, characterId, error));

        while (_failures.Count > Maximum && _failures.TryDequeue(out _))
        {
        }
    }
}
