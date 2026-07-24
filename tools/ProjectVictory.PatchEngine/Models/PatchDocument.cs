namespace ProjectVictory.PatchEngine.Models;

public sealed class PatchDocument
{
    public string Version { get; set; } = string.Empty;

    public string PatchId { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<PatchFile> Files { get; } = [];
}