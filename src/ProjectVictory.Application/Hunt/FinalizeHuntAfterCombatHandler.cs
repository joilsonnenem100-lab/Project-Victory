using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class FinalizeHuntAfterCombatHandler
{
    private readonly IHuntRepository _huntRepository;

    public FinalizeHuntAfterCombatHandler(
        IHuntRepository huntRepository)
    {
        _huntRepository = huntRepository;
    }

    public async Task<FinalizeHuntAfterCombatResult> Handle(
        FinalizeHuntAfterCombatCommand command,
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

        hunt.CompleteAfterCombat();

        return new FinalizeHuntAfterCombatResult(
            hunt.Id,
            true);
    }
}