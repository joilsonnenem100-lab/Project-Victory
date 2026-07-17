namespace ProjectVictory.Application.Combat;

public sealed class CreateCombatEncounterHandler
{
    public CreateCombatEncounterResult Handle(
        CreateCombatEncounterCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.EncounterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "EncounterId is required.");
        }

        if (command.HuntAreaId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "HuntAreaId is required.");
        }

        return new CreateCombatEncounterResult(
            command.EncounterId,
            true);
    }
}