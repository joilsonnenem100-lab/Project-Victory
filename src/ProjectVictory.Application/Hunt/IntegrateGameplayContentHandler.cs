namespace ProjectVictory.Application.Hunt;

public sealed class IntegrateGameplayContentHandler
{
    private readonly ConfigureHuntContentHandler _handler;

    public IntegrateGameplayContentHandler()
    {
        _handler = new ConfigureHuntContentHandler();
    }

    public IntegrateGameplayContentResult Handle(
        IntegrateGameplayContentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var result = _handler.Handle(
            new ConfigureHuntContentCommand(
                command.HuntAreaId,
                command.EncounterId));

        return new IntegrateGameplayContentResult(
            result.HuntAreaId,
            result.Configured);
    }
}