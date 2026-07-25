using System;

namespace ProjectVictory.Domain.Hunt;

public sealed class HuntSession
{
    public Guid Id { get; }

    public Guid CharacterId { get; }

    public HuntSessionStatus Status { get; private set; }

    public DateTime CreatedAt { get; }

    public DateTime? StartedAt { get; private set; }

    public DateTime? StoppedAt { get; private set; }


    public HuntSession(Guid id, Guid characterId)
    {
        if (characterId == Guid.Empty)
        {
            throw new ArgumentException(
                "CharacterId is required.",
                nameof(characterId));
        }

        Id = id;
        CharacterId = characterId;
        CreatedAt = DateTime.UtcNow;
        Status = HuntSessionStatus.Created;
    }


    public void Start()
    {
        if (Status != HuntSessionStatus.Created)
        {
            throw new InvalidOperationException(
                "Only created sessions can start.");
        }

        Status = HuntSessionStatus.Running;
        StartedAt = DateTime.UtcNow;
    }


    public void Stop()
    {
        if (Status != HuntSessionStatus.Running)
        {
            throw new InvalidOperationException(
                "Only running sessions can stop.");
        }

        Status = HuntSessionStatus.Stopped;
        StoppedAt = DateTime.UtcNow;
    }
}
