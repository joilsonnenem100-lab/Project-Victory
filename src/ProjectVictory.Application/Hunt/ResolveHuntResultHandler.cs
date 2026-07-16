using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class ResolveHuntResultHandler
{
    private readonly IHuntRepository _huntRepository;

    public ResolveHuntResultHandler(
        IHuntRepository huntRepository)
    {
        _huntRepository = huntRepository;
    }

    public async Task<ResolveHuntResultResult> Handle(
        ResolveHuntResultCommand command,
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

        if (hunt.State != HuntState.Running)
        {
            throw new InvalidOperationException(
                "Only running hunts can have results resolved.");
        }

        if (command.Success)
        {
            hunt.Complete();

            return new ResolveHuntResultResult(
                hunt.Id,
                true,
                true);
        }

        hunt.Fail();

        return new ResolveHuntResultResult(
            hunt.Id,
            false,
            false);
    }
}