using ProjectVictory.Domain.Combat.TurnBased;

namespace ProjectVictory.Application.Combat.TurnBased;

// PV-COMBAT-011: escolhe o próximo participante ativo (exatamente um por turno, ADR-052).
public interface ITurnScheduler
{
    Combatant Next(CombatSession session);
}

// ADR-006: escolhe a Secondary Action opcional de um participante.
// Para monstros é a Monster AI (M4, PV-MONSTER-010); para o jogador, a configuração dele
// (suprimentos, PV-SUPPLY-010). O orquestrador só pergunta; nunca decide.
public interface ICombatActionSelector
{
    CombatActionKind? SelectSecondaryAction(
        CombatSession session,
        Combatant actor);
}

// Hook opcional chamado depois que uma Secondary Action é executada (ex.: consumir a poção do
// inventário — PV-SUPPLY-010). Não altera o combate.
public interface ICombatActionObserver
{
    void OnActionExecuted(
        CombatSession session,
        CombatTurnAction action,
        CombatActionOutcome outcome);
}

public sealed record CombatRoundReport(
    IReadOnlyList<CombatTurnLogEntry> Log,
    CombatSessionOutcome Outcome);

// PV-COMBAT-010: Combat Orchestrator (ADR-051). Coordena a sequência de turnos (ADR-052) e as duas
// etapas de cada turno (ADR-006). NÃO implementa dano, Skills, IA, Loot nem Experience: pede a
// ação secundária ao ICombatActionSelector, as regras ao ICombatResolver, a ordem ao ITurnScheduler
// e só aplica os resultados ao estado (vida/mana).
public sealed class CombatOrchestrator
{
    // Proteção contra laço infinito numa rodada (ex.: dano zero dos dois lados).
    public const int MaximumTurnsPerRound = 1000;

    private readonly ICombatResolver _resolver;
    private readonly ITurnScheduler _scheduler;
    private readonly ICombatActionSelector _actionSelector;
    private readonly IEnumerable<ICombatActionObserver> _observers;

    public CombatOrchestrator(
        ICombatResolver resolver,
        ITurnScheduler scheduler,
        ICombatActionSelector actionSelector,
        IEnumerable<ICombatActionObserver> observers)
    {
        _resolver = resolver;
        _scheduler = scheduler;
        _actionSelector = actionSelector;
        _observers = observers;
    }

    // Uma "rodada" = o turno do participante ativo e os seguintes, até a vez do jogador voltar
    // (o próximo comando do jogador) ou o combate acabar.
    public CombatRoundReport ExecuteRound(
        CombatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        var log = new List<CombatTurnLogEntry>();

        for (var executed = 0; executed < MaximumTurnsPerRound && !session.IsFinished; executed++)
        {
            ExecuteTurn(session, log);

            if (session.IsFinished)
            {
                break;
            }

            session.AdvanceTo(
                _scheduler.Next(session));

            if (session.ActiveParticipant.Side == CombatSide.Player)
            {
                break;
            }
        }

        return new CombatRoundReport(log, session.Outcome);
    }

    private void ExecuteTurn(
        CombatSession session,
        List<CombatTurnLogEntry> log)
    {
        var actor = session.ActiveParticipant;

        if (actor.IsDead)
        {
            return;
        }

        // Etapa 1 (ADR-006): Basic Attack, sempre.
        var target = session.DefaultTargetFor(actor);

        if (target is null)
        {
            return;
        }

        Apply(session, new CombatTurnAction(actor, target, CombatActionKind.BasicAttack), log);

        if (session.IsFinished)
        {
            return;
        }

        // Etapa 2 (ADR-006): Secondary Action opcional, escolhida fora do orquestrador.
        var secondary = _actionSelector.SelectSecondaryAction(session, actor);

        if (secondary is null)
        {
            return;
        }

        var secondaryTarget = secondary is CombatActionKind.UseHealthPotion or CombatActionKind.UseManaPotion
            ? actor
            : session.DefaultTargetFor(actor);

        if (secondaryTarget is null)
        {
            return;
        }

        Apply(session, new CombatTurnAction(actor, secondaryTarget, secondary.Value), log);
    }

    private void Apply(
        CombatSession session,
        CombatTurnAction action,
        List<CombatTurnLogEntry> log)
    {
        var outcome = _resolver.ResolveAction(action, session.Random);

        if (!outcome.Executed)
        {
            return;
        }

        if (outcome.ManaCost > 0 && !action.Actor.TrySpendMana(outcome.ManaCost))
        {
            return;
        }

        action.Target.ApplyDamage(outcome.Damage);
        action.Actor.Heal(outcome.Healing);
        action.Actor.RestoreMana(outcome.ManaRestored);

        log.Add(new CombatTurnLogEntry(
            session.TurnNumber,
            action.Actor.Id,
            action.Actor.Name,
            action.Target.Id,
            action.Target.Name,
            action.Kind,
            outcome.Damage,
            outcome.Healing,
            action.Target.CurrentHealth,
            action.Actor.CurrentHealth));

        foreach (var observer in _observers)
        {
            observer.OnActionExecuted(session, action, outcome);
        }
    }
}
