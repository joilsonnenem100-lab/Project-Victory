namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidateGameplayRuntimeStateHandler
{
    public ValidateGameplayRuntimeStateResult Handle(
        ValidateGameplayRuntimeStateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.SessionActive &&
            command.HuntActive &&
            command.CombatActive;

        return new ValidateGameplayRuntimeStateResult(
            valid);
    }
}