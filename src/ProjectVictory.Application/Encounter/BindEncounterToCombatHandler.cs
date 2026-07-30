namespace ProjectVictory.Application.Encounter;

public sealed class BindEncounterToCombatHandler
{
    public BindEncounterToCombatResult Handle(
        BindEncounterToCombatCommand command)
    {
        return new BindEncounterToCombatResult(
            command.EncounterId,
            command.CombatContextId);
    }
}
