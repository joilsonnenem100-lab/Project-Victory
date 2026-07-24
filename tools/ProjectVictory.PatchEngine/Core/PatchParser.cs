using ProjectVictory.PatchEngine.Models;

namespace ProjectVictory.PatchEngine.Core;

/// <summary>
/// Compatibilidade temporária.
/// O formato proprietário PVPATCH foi descontinuado.
/// Todo parsing agora é realizado por UnifiedDiffParser.
/// </summary>
[Obsolete("Use UnifiedDiffParser instead.")]
public sealed class PatchParser
{
    private readonly UnifiedDiffParser _parser = new();

    public PatchDocument Parse(string patchFile)
    {
        return _parser.Parse(patchFile);
    }
}