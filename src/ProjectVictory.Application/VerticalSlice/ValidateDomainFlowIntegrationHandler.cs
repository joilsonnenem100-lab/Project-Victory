namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidateDomainFlowIntegrationHandler
{
    private readonly ValidateVerticalSliceBoundariesHandler _handler;

    public ValidateDomainFlowIntegrationHandler()
    {
        _handler = new ValidateVerticalSliceBoundariesHandler();
    }

    public ValidateDomainFlowIntegrationResult Handle(
        ValidateDomainFlowIntegrationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var result = _handler.Handle();

        return new ValidateDomainFlowIntegrationResult(
            result.HuntValid &&
            result.RewardValid &&
            result.ProgressionValid &&
            result.CharacterValid);
    }
}