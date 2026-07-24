namespace ProjectVictory.PatchEngine.Models;

public sealed class PatchHunk
{
    public int OriginalStart { get; init; }

    public int OriginalCount { get; init; }

    public int NewStart { get; init; }

    public int NewCount { get; init; }

    public List<PatchChange> Changes { get; } = [];
}