using System.Text;

namespace ProjectVictory.PatchEngine.Core;

public sealed class UnifiedDiffGenerator
{
    public void Generate(
        IEnumerable<(string Original, string Modified)> files,
        string outputPatch)
    {
        ArgumentNullException.ThrowIfNull(files);

        var sb = new StringBuilder();

        foreach (var file in files)
        {
            AppendFileDiff(
                sb,
                file.Original,
                file.Modified);
        }

        File.WriteAllText(outputPatch, sb.ToString());
    }

    private static void AppendFileDiff(
        StringBuilder sb,
        string originalFile,
        string modifiedFile)
    {
        var original = File.ReadAllLines(originalFile);
        var modified = File.ReadAllLines(modifiedFile);

        var relative = Path.GetFileName(modifiedFile);

        sb.AppendLine($"diff --git a/{relative} b/{relative}");
        sb.AppendLine($"--- a/{relative}");
        sb.AppendLine($"+++ b/{relative}");
        sb.AppendLine($"@@ -1,{original.Length} +1,{modified.Length} @@");

        var max = Math.Max(original.Length, modified.Length);

        for (var i = 0; i < max; i++)
        {
            var oldLine = i < original.Length ? original[i] : null;
            var newLine = i < modified.Length ? modified[i] : null;

            if (oldLine == newLine)
            {
                if (oldLine is not null)
                    sb.AppendLine($" {oldLine}");

                continue;
            }

            if (oldLine is not null)
                sb.AppendLine($"-{oldLine}");

            if (newLine is not null)
                sb.AppendLine($"+{newLine}");
        }

        sb.AppendLine();
    }

    public void Generate(
        string originalFile,
        string modifiedFile,
        string outputPatch)
    {
        Generate(
        [
            (originalFile, modifiedFile)
        ],
        outputPatch);
    }
}