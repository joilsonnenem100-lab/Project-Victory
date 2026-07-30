using System;

namespace ProjectVictory.Domain.Encounter;

public sealed class Encounter
{
    public Guid Id { get; }

    public EncounterStatus Status { get; private set; }

    public Encounter(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Encounter id is required.",
                nameof(id));
        }

        Id = id;
        Status = EncounterStatus.Created;
    }

    public void Activate()
    {
        if (Status != EncounterStatus.Created)
        {
            throw new InvalidOperationException(
                $"Invalid encounter lifecycle transition: {Status} -> Active.");
        }

        Status = EncounterStatus.Active;
    }

    public void Resolve()
    {
        if (Status != EncounterStatus.Active)
        {
            throw new InvalidOperationException(
                $"Invalid encounter lifecycle transition: {Status} -> Resolved.");
        }

        Status = EncounterStatus.Resolved;
    }

    public void Finish()
    {
        if (Status != EncounterStatus.Resolved)
        {
            throw new InvalidOperationException(
                $"Invalid encounter lifecycle transition: {Status} -> Finished.");
        }

        Status = EncounterStatus.Finished;
    }
}
