using ProjectVictory.Application.Events;
using ProjectVictory.Domain.Events;

namespace ProjectVictory.Application.Reward;

// Revisão do PV-EVENT-010/011 (ressalva 4c): o assinante fica no módulo Reward — o módulo Events não
// conhece Reward. ATENÇÃO (events.md: "subscribers nunca são obrigatórios" × R-058): este assinante é
// OBRIGATÓRIO hoje (sem ele, matar monstros não dá XP) — um teste de composição garante o registro.
// PV-EVENT-011: CombatCompletedEvent → Reward Pipeline (ADR-024). O Combat não conhece o Reward.
public sealed class RewardOnCombatCompleted : IAsyncEventHandler<CombatCompletedEvent>
{
    private readonly RewardResolutionHandler _reward;

    public RewardOnCombatCompleted(RewardResolutionHandler reward)
    {
        _reward = reward;
    }

    public async Task HandleAsync(CombatCompletedEvent domainEvent, CancellationToken cancellationToken = default) =>
        await _reward.Handle(domainEvent.ToCombatResult(), cancellationToken);
}
