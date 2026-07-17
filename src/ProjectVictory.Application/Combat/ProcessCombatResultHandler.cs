namespace ProjectVictory.Application.Combat;

public sealed class ProcessCombatResultHandler
{
    public ProcessCombatResultResult Handle(
        ProcessCombatResultCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CombatId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CombatId is required.");
        }

        return new ProcessCombatResultResult(
            command.CombatId,
            true);
    }
}