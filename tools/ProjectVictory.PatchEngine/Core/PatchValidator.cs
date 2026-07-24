using ProjectVictory.PatchEngine.Models;

namespace ProjectVictory.PatchEngine.Core;

public sealed class PatchValidator
{
    public void Validate(PatchDocument patch)
    {
        ArgumentNullException.ThrowIfNull(patch);

        if (patch.Files.Count == 0)
        {
            throw new InvalidOperationException(
                "Patch does not contain any files.");
        }

        foreach (var file in patch.Files)
        {
            if (string.IsNullOrWhiteSpace(file.Path))
            {
                throw new InvalidOperationException(
                    "Patch contains an invalid file path.");
            }

            if (file.Hunks.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Patch '{file.Path}' contains no hunks.");
            }

            foreach (var hunk in file.Hunks)
            {
                if (hunk.Changes.Count == 0)
                {
                    throw new InvalidOperationException(
                        $"Patch '{file.Path}' contains an empty hunk.");
                }
            }
        }
    }
}