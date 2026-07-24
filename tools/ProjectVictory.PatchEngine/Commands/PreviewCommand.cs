using ProjectVictory.PatchEngine.Core;

namespace ProjectVictory.PatchEngine.Commands;

public sealed class PreviewCommand : ICommand
{
    public string Name => "preview";

    public int Execute(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  pvpatch preview <patch-file>");
            return 1;
        }

        var parser = new UnifiedDiffParser();
        var validator = new PatchValidator();

        var patch = parser.Parse(args[0]);

        validator.Validate(patch);

        Console.WriteLine();
        Console.WriteLine("======================================");
        Console.WriteLine("PROJECT VICTORY PATCH PREVIEW");
        Console.WriteLine("======================================");
        Console.WriteLine();

        Console.WriteLine($"Files: {patch.Files.Count}");
        Console.WriteLine();

        foreach (var file in patch.Files)
        {
            var additions = file.Hunks
                .Sum(h => h.Changes.Count(c => c.IsAddition));

            var removals = file.Hunks
                .Sum(h => h.Changes.Count(c => c.IsRemoval));

            Console.WriteLine(file.Path);
            Console.WriteLine($"   +{additions}  -{removals}");
            Console.WriteLine();
        }

        Console.WriteLine("Preview completed.");
        Console.WriteLine("No files were modified.");

        return 0;
    }
}