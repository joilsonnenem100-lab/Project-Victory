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
        if (characterId == Guid.Empty)
        {
            throw new ArgumentException(
                "CharacterId is required.",
                nameof(characterId));
        }

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
                "Only running hunts can be completed.");
        }

        State = HuntState.Completed;
    }

    public void CompleteAfterCombat()
    {
        if (State != HuntState.Running)
        {
            throw new InvalidOperationException(
                "Only running hunts can be completed.");
        }

        State = HuntState.Completed;
    }

    public void Fail()
    {
        if (State != HuntState.Running)
        {
            throw new InvalidOperationException(
                "Only running hunts can fail.");
        }

        State = HuntState.Failed;
    }

    public void Cancel()
    {
        if (State != HuntState.Running)
        {
            throw new InvalidOperationException(
                "Only running hunts can be cancelled.");
        }

        State = HuntState.Cancelled;
    }
}