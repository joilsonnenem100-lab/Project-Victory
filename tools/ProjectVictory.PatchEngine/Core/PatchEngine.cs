using ProjectVictory.PatchEngine.Models;

namespace ProjectVictory.PatchEngine.Core;

public sealed class PatchEngine
{
    private readonly PatchValidator _validator;
    private readonly BackupService _backupService;
    private readonly ReportService _reportService;

    public PatchEngine(
        PatchValidator validator,
        BackupService backupService,
        ReportService reportService)
    {
        _validator = validator;
        _backupService = backupService;
        _reportService = reportService;
    }

    public void Apply(PatchDocument patch)
    {
        ArgumentNullException.ThrowIfNull(patch);

        _validator.Validate(patch);

        _backupService.CreateBackup(
            patch.Files.Select(x => new PatchFile
            {
                Path = x.Path,
                IsNewFile = x.IsNewFile,
                IsDeletedFile = x.IsDeletedFile
            }));

        foreach (var file in patch.Files)
        {
            ApplyFile(file);
        }

        _reportService.BackupCreated = true;
        _reportService.PatchApplied = true;

        _reportService.Print();
    }

    private void ApplyFile(PatchFile patchFile)
    {
        Console.WriteLine($"Applying: {patchFile.Path}");

        var lines = File.Exists(patchFile.Path)
            ? File.ReadAllLines(patchFile.Path).ToList()
            : new List<string>();

        foreach (var hunk in patchFile.Hunks)
        {
            ApplyHunk(lines, hunk);
        }

        var directory = Path.GetDirectoryName(patchFile.Path);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllLines(patchFile.Path, lines);

        _reportService.FileCount++;
    }

    private void ApplyHunk(
        List<string> lines,
        PatchHunk hunk)
    {
        var index = hunk.OriginalStart - 1;

        foreach (var change in hunk.Changes)
        {
            switch (change.Operation)
            {
                case ' ':

                    if (index >= lines.Count)
                    {
                        throw new InvalidOperationException(
                            "Patch context outside file.");
                    }

                    if (lines[index] != change.Text)
                    {
                        throw new InvalidOperationException(
                            $"Patch context mismatch at line {index + 1}.");
                    }

                    index++;

                    break;

                case '-':

                    if (index >= lines.Count)
                    {
                        throw new InvalidOperationException(
                            "Patch removal outside file.");
                    }

                    if (lines[index] != change.Text)
                    {
                        throw new InvalidOperationException(
                            $"Patch removal mismatch at line {index + 1}.");
                    }

                    lines.RemoveAt(index);

                    _reportService.RemovedLines++;

                    break;

                case '+':

                    lines.Insert(index, change.Text);

                    index++;

                    _reportService.AddedLines++;

                    break;

                default:

                    throw new InvalidOperationException(
                        $"Unsupported patch operation '{change.Operation}'.");
            }
        }
    }
}