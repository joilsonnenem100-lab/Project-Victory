namespace ProjectVictory.API.Database;

// PV-PLATFORM-004: quando o startup aplica as migrations existentes.
// - Development: sempre (PV-PLATFORM-001, inalterado).
// - Fora de Development: só com "Database:ApplyMigrationsOnStartup" = true (padrão: false).
public static class DatabaseStartupSettings
{
    public const string ApplyMigrationsOnStartupKey = "Database:ApplyMigrationsOnStartup";

    public static bool ShouldApplyMigrations(
        bool isDevelopment,
        bool applyMigrationsOnStartup)
    {
        return isDevelopment || applyMigrationsOnStartup;
    }
}
