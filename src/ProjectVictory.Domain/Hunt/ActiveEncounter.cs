using System;
using System.Collections.Generic;
using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Domain.Hunt;

public sealed class ActiveEncounter
{
    private readonly List<EncounterMonsterState> _monsters;


    public Guid Id { get; }

    public Guid HuntId { get; }


    public IReadOnlyCollection<EncounterMonsterState> Monsters =>
        _monsters;


    public Guid CurrentTargetId { get; private set; }


    public CombatState State { get; private set; }

    // PV-COMBAT-011: semente do combate deste encounter (combate reproduzível, ADR-052).
    public int Seed { get; }

    // PV-COMBAT-012: combate por turnos em andamento neste encounter (estado temporário, em memória).
    public ProjectVictory.Domain.Combat.TurnBased.CombatSession? Session { get; private set; }



    public ActiveEncounter(
        Guid id,
        Guid huntId,
        IReadOnlyCollection<EncounterMonsterState> monsters)
        : this(id, huntId, monsters, 0)
    {
    }

    public ActiveEncounter(
        Guid id,
        Guid huntId,
        IReadOnlyCollection<EncounterMonsterState> monsters,
        int seed)
    {
        Seed = seed;

        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Encounter id is required.",
                nameof(id));
        }


        if (huntId == Guid.Empty)
        {
            throw new ArgumentException(
                "Hunt id is required.",
                nameof(huntId));
        }


        ArgumentNullException.ThrowIfNull(
            monsters);


        if (monsters.Count == 0)
        {
            throw new InvalidOperationException(
                "Encounter requires at least one monster.");
        }


        Id = id;

        HuntId = huntId;


        _monsters =
            new List<EncounterMonsterState>(
                monsters);


        CurrentTargetId =
            _monsters[0].Id;


        State =
            CombatState.Created;
    }



    public EncounterMonsterState GetCurrentTarget()
    {
        var target =
            _monsters.Find(
                monster =>
                    monster.Id == CurrentTargetId);


        if (target is null)
        {
            throw new InvalidOperationException(
                "Current target not found.");
        }


        return target;
    }



    public void AttachSession(
        ProjectVictory.Domain.Combat.TurnBased.CombatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        if (Session is not null)
        {
            throw new InvalidOperationException(
                "Encounter already has a combat session.");
        }

        Session = session;
    }

    public void Activate()
    {
        if (State == CombatState.Created)
        {
            State =
                CombatState.Active;
        }
    }



    public void Victory()
    {
        State =
            CombatState.Victory;
    }



    public void Defeat()
    {
        State =
            CombatState.Defeat;
    }



    public bool IsFinished()
    {
        return State == CombatState.Victory ||
               State == CombatState.Defeat;
    }



    public void SelectNextTarget()
    {
        var next =
            _monsters.Find(
                monster =>
                    !monster.IsDead);


        if (next is null)
        {
            return;
        }


        CurrentTargetId =
            next.Id;
    }



    public bool IsCompleted()
    {
        foreach (var monster in _monsters)
        {
            if (!monster.IsDead)
            {
                return false;
            }
        }


        return true;
    }
}