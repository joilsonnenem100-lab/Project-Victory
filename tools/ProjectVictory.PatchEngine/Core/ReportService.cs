namespace ProjectVictory.PatchEngine.Core;

public sealed class ReportService
{
    public int FileCount { get; set; }

    public int AddedLines { get; set; }

    public int RemovedLines { get; set; }

    public bool BackupCreated { get; set; }

    public bool PatchApplied { get; set; }

    public int ConflictCount { get; set; }

    public void Print()
    {
        Console.WriteLine();
        Console.WriteLine("======================================");
        Console.WriteLine("PROJECT VICTORY PATCH REPORT");
        Console.WriteLine("======================================");
        Console.WriteLine();

        Console.WriteLine($"Files Modified : {FileCount}");
        Console.WriteLine($"Lines Added    : {AddedLines}");
        Console.WriteLine($"Lines Removed  : {RemovedLines}");
        Console.WriteLine($"Backup Created : {BackupCreated}");
        Console.WriteLine($"Patch Applied  : {PatchApplied}");
        Console.WriteLine($"Conflicts      : {ConflictCount}");

        Console.WriteLine();
        Console.WriteLine("======================================");
    }
}