using System.Text.RegularExpressions;
using ProjectVictory.PatchEngine.Models;

namespace ProjectVictory.PatchEngine.Core;

public sealed class UnifiedDiffParser
{
    private static readonly Regex HunkRegex =
        new(@"^@@\s+\-(\d+),?(\d*)\s+\+(\d+),?(\d*)\s+@@");

    public PatchDocument Parse(string patchFile)
    {
        if (!File.Exists(patchFile))
        {
            throw new FileNotFoundException(
                "Patch file not found.",
                patchFile);
        }

        var document = new PatchDocument();

        PatchFile? currentFile = null;
        PatchHunk? currentHunk = null;

        foreach (var rawLine in File.ReadLines(patchFile))
        {
            if (rawLine.StartsWith("--- "))
            {
                continue;
            }

            if (rawLine.StartsWith("diff --git "))
            {
                currentFile = null;
                currentHunk = null;
                continue;
            }

            if (rawLine.StartsWith("+++ "))
            {
                var path = rawLine[4..].Trim();

                if (path.StartsWith("b/"))
                {
                    path = path[2..];
                }

                currentFile = new PatchFile
                {
                    Path = path
                };

                document.Files.Add(currentFile);

                continue;
            }

            if (currentFile is null)
            {
                continue;
            }

            var match = HunkRegex.Match(rawLine);

            if (match.Success)
            {
                currentHunk = new PatchHunk
                {
                    OriginalStart = int.Parse(match.Groups[1].Value),
                    OriginalCount = string.IsNullOrWhiteSpace(match.Groups[2].Value)
                        ? 1
                        : int.Parse(match.Groups[2].Value),
                    NewStart = int.Parse(match.Groups[3].Value),
                    NewCount = string.IsNullOrWhiteSpace(match.Groups[4].Value)
                        ? 1
                        : int.Parse(match.Groups[4].Value)
                };

                currentFile.Hunks.Add(currentHunk);

                continue;
            }

            if (currentHunk is null)
            {
                continue;
            }

            if (rawLine.Length == 0)
            {
                currentHunk.Changes.Add(new PatchChange
                {
                    Operation = ' ',
                    Text = string.Empty
                });

                continue;
            }

            var operation = rawLine[0];

            if (operation is '+' or '-' or ' ')
            {
                currentHunk.Changes.Add(new PatchChange
                {
                    Operation = operation,
                    Text = rawLine.Length > 1
                        ? rawLine[1..]
                        : string.Empty
                });
            }
        }

        return document;
    }
}