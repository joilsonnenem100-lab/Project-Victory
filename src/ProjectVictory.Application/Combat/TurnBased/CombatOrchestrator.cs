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

    // PV-COMBAT-013: escolha completa (tipo + skill). Padrão: só o tipo, sem skill (aditivo).
    // Atenção (revisão do PV-COMBAT-013, ressalva 5): um seletor que devolva UseSkill só por
    // SelectSecondaryAction fica sem skill e o resolver devolve NotExecuted (seguro, mas sem efeito);
    // para usar skills, implemente este método.
    CombatTurnChoice? SelectSecondaryChoice(
        CombatSession session,
        Combatant actor) =>
        SelectSecondaryAction(session, actor) is { } kind ? new CombatTurnChoice(kind) : null;
}

// PV-COMBAT-013: a Secondary Action escolhida, com a skill quando Kind = UseSkill.
public sealed record CombatTurnChoice(
    CombatActionKind Kind,
    ProjectVictory.Domain.Combat.Rules.SkillDefinition? Skill = null);

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
    CombatSessionOutcome Outcome,
    // Revisão do PV-COMBAT-010: true quando a rodada parou pelo limite de segurança de turnos.
    bool TurnLimitReached = false);

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
        var executed = 0;
        var returnedToPlayer = false;

        for (; executed < MaximumTurnsPerRound && !session.IsFinished; executed++)
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
                returnedToPlayer = true;
                break;
            }
        }

        var limitReached = !session.IsFinished && !returnedToPlayer && executed >= MaximumTurnsPerRound;

        return new CombatRoundReport(log, session.Outcome, limitReached);
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
        var choice = _actionSelector.SelectSecondaryChoice(session, actor);

        if (choice is null)
        {
            return;
        }

        // PV-COMBAT-013: skill em área atinge todos os inimigos vivos (mana gasta uma vez só).
        if (choice.Kind == CombatActionKind.UseSkill
            && choice.Skill is { Targeting: ProjectVictory.Domain.Combat.Rules.SkillTargeting.Area } areaSkill)
        {
            ApplyArea(session, actor, areaSkill, log);
            return;
        }

        // *** PROVISÓRIO *** (revisão do PV-COMBAT-010): poções miram o próprio ator e cura/mana
        // vão para o ator; cura em aliado e escolha de alvo por IA ficam para o M4.
        var secondaryTarget = choice.Kind is CombatActionKind.UseHealthPotion or CombatActionKind.UseManaPotion
            ? actor
            : session.DefaultTargetFor(actor);

        if (secondaryTarget is null)
        {
            return;
        }

        Apply(session, new CombatTurnAction(actor, secondaryTarget, choice.Kind, choice.Skill), log);
    }

    // PV-COMBAT-013 (revisão B1/B2, ADR-051): o orquestrador NÃO decide a regra da skill em área.
    // Passa os oponentes vivos ao resolver, cobra a mana UMA vez (o custo que o resolver devolveu)
    // e aplica o resultado de cada alvo.
    private void ApplyArea(
        CombatSession session,
        Combatant actor,
        ProjectVictory.Domain.Combat.Rules.SkillDefinition skill,
        List<CombatTurnLogEntry> log)
    {
        var opponents = actor.Side == CombatSide.Player
            ? session.Monsters.Where(m => !m.IsDead).ToList()
            : session.Player.IsDead ? new List<Combatant>() : new List<Combatant> { session.Player };

        var resolution = _resolver.ResolveSkill(actor, skill, opponents, session.Random);

        if (!resolution.Executed)
        {
            return;
        }

        if (resolution.ManaCost > 0 && !actor.TrySpendMana(resolution.ManaCost))
        {
            return;
        }

        foreach (var hit in resolution.Hits)
        {
            Record(session, new CombatTurnAction(actor, hit.Target, CombatActionKind.UseSkill, skill), hit.Outcome, log);
        }
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

        Record(session, action, outcome, log);
    }

    // Aplica o resultado já calculado (dano, cura, mana) e registra no log.
    private void Record(
        CombatSession session,
        CombatTurnAction action,
        CombatActionOutcome outcome,
        List<CombatTurnLogEntry> log)
    {
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
            action.Actor.CurrentHealth,
            action.Skill?.Name));

        foreach (var observer in _observers)
        {
            observer.OnActionExecuted(session, action, outcome);
        }
    }
}
