using System;

namespace ProjectVictory.Domain.Hunt;

public sealed class Hunt
{
    public Guid Id { get; }

    public Guid CharacterId { get; }

    public Guid RuntimeId { get; }

    public HuntState State { get; private set; }

    // PV-HUNT-011: medição da Hunt para o XP/hora do Analyzer (início, fim, XP ganha na Hunt).
    public DateTimeOffset? StartedAt { get; private set; }

    public DateTimeOffset? EndedAt { get; private set; }

    public long ExperienceGained { get; private set; }

    public TimeSpan ElapsedAt(DateTimeOffset now) =>
        StartedAt is { } started ? (EndedAt ?? now) - started : TimeSpan.Zero;


    public Hunt(
        Guid id,
        Guid characterId,
        Guid runtimeId)
    {
        if (characterId == Guid.Empty)
        {
            throw new ArgumentException(
                "CharacterId is required.",
                nameof(characterId));
        }

        if (runtimeId == Guid.Empty)
        {
            throw new ArgumentException(
                "RuntimeId is required.",
                nameof(runtimeId));
        }

        Id = id;
        CharacterId = characterId;
        RuntimeId = runtimeId;
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

    // PV-HUNT-011: início com horário (o Start() antigo continua, sem horário).
    public void Start(DateTimeOffset now)
    {
        Start();
        StartedAt = now;
    }

    // PV-HUNT-011: horário de fim (uma vez; a Hunt já precisa estar encerrada).
    public void MarkEnded(DateTimeOffset now)
    {
        if (State == HuntState.Running || State == HuntState.Created)
        {
            throw new InvalidOperationException("Only ended hunts can be marked as ended.");
        }

        EndedAt ??= now;
    }

    public void RecordExperience(long amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        ExperienceGained = checked(ExperienceGained + amount);
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