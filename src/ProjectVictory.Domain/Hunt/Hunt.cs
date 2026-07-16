using System;

namespace ProjectVictory.Domain.Hunt;

public sealed class Hunt
{
    public Guid Id { get; }

    public Guid CharacterId { get; }

    public HuntState State { get; private set; }

    public Hunt(
        Guid id,
        Guid characterId)
    {
        Id = id;
        CharacterId = characterId;
        State = HuntState.Created;
    }

    public void Start()
    {
        if (State != HuntState.Created)
        {
            throw new InvalidOperationException(
                "Hunt cannot start from current state.");
        }

        State = HuntState.Running;
    }

    public void Complete()
    {
        if (State != HuntState.Running)
        {
            throw new InvalidOperationException(
                "Hunt cannot complete from current state.");
        }

        State = HuntState.Completed;
    }

    public void Fail()
    {
        if (State != HuntState.Running)
        {
            throw new InvalidOperationException(
                "Hunt cannot fail from current state.");
        }

        State = HuntState.Failed;
    }

    public void Cancel()
    {
        if (State != HuntState.Running)
        {
            throw new InvalidOperationException(
                "Hunt cannot cancel from current state.");
        }

        State = HuntState.Cancelled;
    }
}