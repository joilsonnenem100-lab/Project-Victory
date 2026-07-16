namespace ProjectVictory.Application.Progression;

public sealed class ValidateProgressionRecoveryHandler
{
    private readonly IProgressionStateRepository _repository;

    public ValidateProgressionRecoveryHandler(
        IProgressionStateRepository repository)
    {
        _repository = repository;
    }

    public async Task<ValidateProgressionRecoveryResult> Handle(
        ValidateProgressionRecoveryCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var state = await _repository.GetAsync(
            command.CharacterId,
            cancellationToken);

        return new ValidateProgressionRecoveryResult(
            command.CharacterId,
            state is not null);
    }
}