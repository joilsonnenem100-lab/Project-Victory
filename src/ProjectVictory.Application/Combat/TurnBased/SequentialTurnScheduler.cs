using ProjectVictory.Domain.Combat.TurnBased;

namespace ProjectVictory.Application.Combat.TurnBased;

// PV-COMBAT-011: ordem de turnos determinística (ADR-052): jogador, depois cada monstro vivo na
// ordem do encounter, e de novo o jogador. Exatamente um participante ativo por turno; mortos são
// pulados. A ordem não usa aleatoriedade. *** PROVISÓRIO *** — iniciativa/velocidade não estão
// documentadas; o jogador sempre começa.
public sealed class SequentialTurnScheduler : ITurnScheduler
{
    public Combatant Next(CombatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        var participants = session.Participants;
        var current = IndexOf(participants, session.ActiveParticipant);

        for (var step = 1; step <= participants.Count; step++)
        {
            var candidate = participants[(current + step) % participants.Count];

            if (!candidate.IsDead)
            {
                return candidate;
            }
        }

        return session.ActiveParticipant;
    }

    private static int IndexOf(IReadOnlyList<Combatant> participants, Combatant combatant)
    {
        for (var i = 0; i < participants.Count; i++)
        {
            if (ReferenceEquals(participants[i], combatant))
            {
                return i;
            }
        }

        throw new InvalidOperationException("Active participant is not in the combat.");
    }
}

// PV-COMBAT-010: seletor sem Secondary Action. *** PROVISÓRIO *** — a Monster AI é o
// PV-MONSTER-010 (M4) e a escolha do jogador vem do PV-SUPPLY-010.
public sealed class NoSecondaryActionSelector : ICombatActionSelector
{
    public CombatActionKind? SelectSecondaryAction(
        CombatSession session,
        Combatant actor) => null;
}
