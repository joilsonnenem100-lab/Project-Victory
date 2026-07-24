using ProjectVictory.PatchEngine.Core;

namespace ProjectVictory.PatchEngine.Commands;

public sealed class GenerateCommand : ICommand
{
    public string Name => "generate";

    public int Execute(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  pvpatch generate <original> <modified> <output.patch>");
            return 1;
        }

        var generator = new UnifiedDiffGenerator();

        generator.Generate(
            args[0],
            args[1],
            args[2]);

        Console.WriteLine();
        Console.WriteLine($"Patch generated: {args[2]}");

        return 0;
    }
}