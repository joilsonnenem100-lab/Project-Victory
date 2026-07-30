namespace ProjectVictory.Application.Session;

public sealed class ValidateVerticalSliceSessionFlowCommand
{
    public Guid CharacterId { get; }
    public Guid RuntimeId { get; }

    public ValidateVerticalSliceSessionFlowCommand(
        Guid characterId,
        Guid runtimeId)
    {
        CharacterId = characterId;
        RuntimeId = runtimeId;
    }
}
