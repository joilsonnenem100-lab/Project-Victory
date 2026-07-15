using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class CancelHuntHandler
{
    private readonly IHuntRepository _huntRepository;

    public CancelHuntHandler(
        IHuntRepository huntRepository)
    {
        _huntRepository = huntRepository;
    }

    public async Task<CancelHuntResult> Handle(
        CancelHuntCommand command,
        CancellationToken cancellationToken = default)
    {
        var hunt = await _huntRepository.GetByIdAsync(
            command.HuntId,
            cancellationToken);

        if (hunt is null)
        {
            throw new InvalidOperationException(
                "Hunt not found.");
        }

        hunt.Cancel();

        return new CancelHuntResult(
            hunt.Id,
            true);
    }
}