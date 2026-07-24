using ProjectVictory.PatchEngine.Core;

namespace ProjectVictory.PatchEngine.Commands;

public sealed class RollbackCommand : ICommand
{
    public string Name => "rollback";

    public int Execute(string[] args)
    {
        var backupService = new BackupService();

        var latestBackup = backupService.GetLatestBackup();

        if (latestBackup is null)
        {
            Console.WriteLine("No backup found.");
            return 1;
        }

        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine("PROJECT VICTORY PATCH ROLLBACK");
        Console.WriteLine("========================================");
        Console.WriteLine();

        Console.WriteLine($"Latest Backup: {latestBackup}");
        Console.WriteLine();
        Console.WriteLine("Rollback implementation will be completed in the next delivery.");
        Console.WriteLine();

        return 0;
    }
}