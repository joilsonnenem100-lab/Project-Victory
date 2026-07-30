using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Application.World;
using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.Runtime;

public sealed class ChangeRuntimeLocationHandler
{
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;
    private readonly IWorldDefinitionStore _worldDefinitionStore;

    public ChangeRuntimeLocationHandler(
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        IWorldDefinitionStore worldDefinitionStore)
    {
        _runtimeExecutionStateStore = runtimeExecutionStateStore;
        _worldDefinitionStore = worldDefinitionStore;
    }

    public ChangeRuntimeLocationResult Handle(
        ChangeRuntimeLocationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (!_worldDefinitionStore.Exists(command.WorldId))
            throw new InvalidOperationException("World not found.");

        if (!_worldDefinitionStore.ContainsRegion(
                command.WorldId,
                command.RegionId))
            throw new InvalidOperationException("Region does not belong to World.");

        if (!_worldDefinitionStore.ContainsLocation(
                command.RegionId,
                command.LocationId))
            throw new InvalidOperationException("Location does not belong to Region.");

        var currentContext =
            _runtimeExecutionStateStore.Get(
                command.RuntimeId);

        var worldContext =
            new RuntimeWorldContext(
                command.WorldId,
                command.RegionId,
                command.LocationId);

        var updatedContext =
            currentContext with
            {
                WorldContext = worldContext
            };

        _runtimeExecutionStateStore.Store(updatedContext);

        return new ChangeRuntimeLocationResult(
            updatedContext.RuntimeId,
            worldContext.LocationId);
    }
}
