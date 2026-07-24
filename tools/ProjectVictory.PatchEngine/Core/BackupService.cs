using ProjectVictory.PatchEngine.Models;

namespace ProjectVictory.PatchEngine.Core;

public sealed class BackupService
{
    private const string BackupRoot = "Backup";

    public void CreateBackup(IEnumerable<PatchFile> files)
    {
        ArgumentNullException.ThrowIfNull(files);

        var backupFolder = Path.Combine(
            BackupRoot,
            DateTime.UtcNow.ToString("yyyyMMdd_HHmmss"));

        Directory.CreateDirectory(backupFolder);

        foreach (var file in files)
        {
            if (!File.Exists(file.Path))
            {
                continue;
            }

            var destination = Path.Combine(
                backupFolder,
                file.Path);

            var destinationDirectory = Path.GetDirectoryName(destination);

            if (!string.IsNullOrWhiteSpace(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            File.Copy(
                file.Path,
                destination,
                overwrite: true);
        }

        File.WriteAllText(
            Path.Combine(backupFolder, "backup.info"),
            $"Created={DateTime.UtcNow:O}");
    }

    public string? GetLatestBackup()
    {
        if (!Directory.Exists(BackupRoot))
        {
            return null;
        }

        return Directory
            .GetDirectories(BackupRoot)
            .OrderByDescending(x => x)
            .FirstOrDefault();
    }
}