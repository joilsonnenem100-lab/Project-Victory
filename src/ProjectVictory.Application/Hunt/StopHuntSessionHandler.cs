using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class StopHuntSessionHandler
{
    private readonly IHuntSessionRepository _repository;


    public StopHuntSessionHandler(
        IHuntSessionRepository repository)
    {
        _repository = repository;
    }


    public async Task Handle(
        StopHuntSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        var session =
            await _repository.GetByIdAsync(
                command.HuntSessionId,
                cancellationToken);

        if (session is null)
        {
            throw new InvalidOperationException(
                "Hunt session not found.");
        }

        session.Stop();
    }
}
