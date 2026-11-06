using System;
using System.Collections.Generic;
using System.Linq;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.Combat.TurnBased;

// PV-COMBAT-010/011: estado de um combate por turnos. Exatamente um participante ativo por
// turno (ADR-052), escolhido pelo ITurnScheduler. Toda aleatoriedade vem do IRandomSource
// injetado (com semente, para combates reproduzíveis). Sem regra de dano, IA, loot ou XP.
public sealed class CombatSession
{
    private readonly List<Combatant> _participants;

    public Guid Id { get; }

    public IReadOnlyList<Combatant> Participants => _participants;

    public Combatant Player { get; }

    public IReadOnlyList<Combatant> Monsters { get; }

    public IRandomSource Random { get; }

    public int TurnNumber { get; private set; }

    public Combatant ActiveParticipant { get; private set; }

    public CombatSessionOutcome Outcome
    {
        get
        {
            if (Player.IsDead)
            {
                return CombatSessionOutcome.Defeat;
            }

            return Monsters.All(m => m.IsDead)
                ? CombatSessionOutcome.Victory
                : CombatSessionOutcome.Ongoing;
        }
    }

    public bool IsFinished => Outcome != CombatSessionOutcome.Ongoing;

    public CombatSession(
        Guid id,
        Combatant player,
        IReadOnlyList<Combatant> monsters,
        IRandomSource random)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Combat session id is required.", nameof(id));
        }

        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(monsters);
        ArgumentNullException.ThrowIfNull(random);

        if (player.Side != CombatSide.Player || monsters.Count == 0 || monsters.Any(m => m.Side != CombatSide.Monster))
        {
            throw new ArgumentException("A combat needs one player and at least one monster.");
        }

        Id = id;
        Player = player;
        Monsters = monsters.ToArray();
        Random = random;
        _participants = new List<Combatant> { player };
        _participants.AddRange(Monsters);

        // O jogador sempre abre o combate (PROVISÓRIO; ordem de iniciativa não documentada).
        ActiveParticipant = player;
        TurnNumber = 1;
    }

    // Avança para o próximo participante ativo indicado pelo scheduler (um por turno).
    public void AdvanceTo(Combatant next)
    {
        ArgumentNullException.ThrowIfNull(next);

        if (!_participants.Contains(next))
        {
            throw new ArgumentException("Participant is not in this combat.", nameof(next));
        }

        ActiveParticipant = next;
        TurnNumber++;
    }

    // Alvo padrão do Basic Attack: o jogador ataca o primeiro monstro vivo (ordem do encounter);
    // monstros atacam o jogador.
    public Combatant? DefaultTargetFor(Combatant actor)
    {
        ArgumentNullException.ThrowIfNull(actor);

        return actor.Side == CombatSide.Player
            ? Monsters.FirstOrDefault(m => !m.IsDead)
            : Player.IsDead ? null : Player;
    }
}
