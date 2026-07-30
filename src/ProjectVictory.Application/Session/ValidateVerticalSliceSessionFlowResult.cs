namespace ProjectVictory.Application.Session;

public sealed class ValidateVerticalSliceSessionFlowResult
{
    public Guid CharacterId { get; }
    public Guid RuntimeId { get; }
    public bool IsValid { get; }

    public ValidateVerticalSliceSessionFlowResult(
        Guid characterId,
        Guid runtimeId,
        bool isValid)
    {
        CharacterId = characterId;
        RuntimeId = runtimeId;
        IsValid = isValid;
    }
}
