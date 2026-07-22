using System;
using System.Collections.Generic;

namespace ProjectVictory.Domain.Hunt;

public sealed class ActiveEncounter
{
    private readonly List<EncounterMonsterState> _monsters;


    public Guid Id { get; }

    public Guid HuntId { get; }

    public IReadOnlyCollection<EncounterMonsterState> Monsters =>
        _monsters;


    public Guid CurrentTargetId { get; private set; }


    public ActiveEncounter(
        Guid id,
        Guid huntId,
        IReadOnlyCollection<EncounterMonsterState> monsters)
    {
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


        ArgumentNullException.ThrowIfNull(monsters);


        if (monsters.Count == 0)
        {
            throw new InvalidOperationException(
                "Encounter requires at least one monster.");
        }


        Id = id;

        HuntId = huntId;

        _monsters = new List<EncounterMonsterState>(
            monsters);


        CurrentTargetId =
            _monsters[0].Id;
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