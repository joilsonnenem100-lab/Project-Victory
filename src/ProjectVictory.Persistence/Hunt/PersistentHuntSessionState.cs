using System;

namespace ProjectVictory.Persistence.Hunt;

public sealed class PersistentHuntSessionState
{
    public Guid Id { get; set; }

    public Guid CharacterId { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? StoppedAt { get; set; }
}
