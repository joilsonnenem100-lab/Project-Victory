using ProjectVictory.Domain.Entities;

namespace ProjectVictory.Application.Character;

public sealed class CreateEvolutionSnapshotHandler
{
    public CreateEvolutionSnapshotResult Handle(
        CreateEvolutionSnapshotCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        if (command.Level < 1)
        {
            throw new InvalidOperationException(
                "Level is invalid.");
        }

        var snapshot = new EvolutionSnapshot(
            command.CharacterId,
            command.Level,
            command.HasEvolution);

        return new CreateEvolutionSnapshotResult(
            snapshot.CharacterId,
            true);
    }
}