namespace ProjectVictory.PatchEngine.Models;

public sealed class PatchFile
{
    public string Path { get; set; } = string.Empty;

    public bool IsNewFile { get; set; }

    public bool IsDeletedFile { get; set; }

    public List<PatchHunk> Hunks { get; } = [];
}