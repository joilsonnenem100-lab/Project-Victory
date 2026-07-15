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
        State = HuntState.Running;
    }

    public void Complete()
    {
        State = HuntState.Completed;
    }

    public void Fail()
    {
        State = HuntState.Failed;
    }
}