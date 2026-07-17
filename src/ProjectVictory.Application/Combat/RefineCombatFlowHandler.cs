namespace ProjectVictory.Application.Combat;

public sealed class RefineCombatFlowHandler
{
    private readonly ProcessCombatResultHandler _handler;

    public RefineCombatFlowHandler()
    {
        _handler = new ProcessCombatResultHandler();
    }

    public RefineCombatFlowResult Handle(
        RefineCombatFlowCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var result = _handler.Handle(
            new ProcessCombatResultCommand(
                command.CombatId));

        return new RefineCombatFlowResult(
            result.CombatId,
            result.Processed);
    }
}