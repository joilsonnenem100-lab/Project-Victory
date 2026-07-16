using System;

namespace ProjectVictory.Domain.Combat;

public sealed class CombatContext
{
    public Guid Id { get; }

    public Guid HuntId { get; }

    public CombatState State { get; private set; }

    public bool IsFinalized { get; private set; }

    public CombatContext(
        Guid id,
        Guid huntId)
    {
        if (huntId == Guid.Empty)
        {
            throw new ArgumentException(
                "HuntId is required.",
                nameof(huntId));
        }

        Id = id;
        HuntId = huntId;
        State = CombatState.Created;
        IsFinalized = false;
    }

    public void Activate()
    {
        if (State != CombatState.Created)
        {
            throw new InvalidOperationException(
                "Combat cannot be activated from current state.");
        }

        State = CombatState.Active;
    }

    public CombatActionResult ProcessAction(
        CombatAction action)
    {
        if (State != CombatState.Active)
        {
            throw new InvalidOperationException(
                "Combat context must be active.");
        }

        ArgumentNullException.ThrowIfNull(action);

        return new CombatActionResult(
            Id,
            true);
    }

    public CombatActionExecutionResult ExecuteAction(
        CombatAction action)
    {
        if (State != CombatState.Active)
        {
            throw new InvalidOperationException(
                "Combat context must be active.");
        }

        ArgumentNullException.ThrowIfNull(action);

        return new CombatActionExecutionResult(
            Guid.NewGuid(),
            Id);
    }

    public void ApplyResolution()
    {
        if (State != CombatState.Active)
        {
            throw new InvalidOperationException(
                "Combat context must be active.");
        }

        State = CombatState.Completed;
    }

    public void FinalizeCombat()
    {
        if (State != CombatState.Completed)
        {
            throw new InvalidOperationException(
                "Combat must be completed before finalization.");
        }

        IsFinalized = true;
    }

    public void Complete()
    {
        if (State != CombatState.Active)
        {
            throw new InvalidOperationException(
                "Combat cannot be completed from current state.");
        }

        State = CombatState.Completed;
    }

    public void Cancel()
    {
        if (State != CombatState.Active)
        {
            throw new InvalidOperationException(
                "Combat cannot be cancelled from current state.");
        }

        State = CombatState.Cancelled;
    }
}