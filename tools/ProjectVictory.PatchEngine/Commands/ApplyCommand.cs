using ProjectVictory.PatchEngine.Core;

namespace ProjectVictory.PatchEngine.Commands;

public sealed class ApplyCommand : ICommand
{
    public string Name => "apply";

    public int Execute(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  pvpatch apply <patch-file>");
            return 1;
        }

        var parser = new UnifiedDiffParser();

        var patch = parser.Parse(args[0]);

        var validator = new PatchValidator();
        var backupService = new BackupService();
        var reportService = new ReportService();

        var engine = new ProjectVictory.PatchEngine.Core.PatchEngine(
            validator,
            backupService,
            reportService);

        engine.Apply(patch);

        return 0;
    }
}