using Microsoft.Data.Sqlite;
using ProjectVictory.API.Database;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-PLATFORM-004: fora de Development, as migrations só são aplicadas no startup com
// Database:ApplyMigrationsOnStartup=true (padrão: desligada). Development continua aplicando sempre.
[Collection("Api")]
public sealed class MigrationsOnStartupTests
{
    private const string ApplyVariable = "Database__ApplyMigrationsOnStartup";

    [Theory]
    [InlineData(true, false, true)]
    [InlineData(true, true, true)]
    [InlineData(false, false, false)]
    [InlineData(false, true, true)]
    public void Rule(bool isDevelopment, bool flag, bool expected)
    {
        Assert.Equal(expected, DatabaseStartupSettings.ShouldApplyMigrations(isDevelopment, flag));
    }

    private static int AppliedMigrations(string databasePath)
    {
        SqliteConnection.ClearAllPools();

        using var connection = new SqliteConnection($"Data Source={databasePath};Mode=ReadOnly;Pooling=False");
        connection.Open();

        using var exists = connection.CreateCommand();
        exists.CommandText = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='__EFMigrationsHistory'";
        if (Convert.ToInt32(exists.ExecuteScalar()) == 0)
        {
            return 0;
        }

        using var count = connection.CreateCommand();
        count.CommandText = "SELECT count(*) FROM __EFMigrationsHistory";
        return Convert.ToInt32(count.ExecuteScalar());
    }

    private static IEnumerable<Exception> Flatten(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            yield return current;
        }
    }

    [Fact]
    public async Task Production_WithFlagOn_AppliesMigrations_AndServes()
    {
        var previous = Environment.GetEnvironmentVariable(ApplyVariable);
        Environment.SetEnvironmentVariable(ApplyVariable, "true");

        try
        {
            using var factory = new ApiFactory("Production");
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/playable/status");

            // A API subiu (schema criado e seed executado): sem token, 401.
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.True(AppliedMigrations(factory.DatabasePath) > 0);
        }
        finally
        {
            Environment.SetEnvironmentVariable(ApplyVariable, previous);
        }
    }

    [Fact]
    public void Production_WithFlagOff_DoesNotApplyMigrations()
    {
        var previous = Environment.GetEnvironmentVariable(ApplyVariable);
        Environment.SetEnvironmentVariable(ApplyVariable, null); // padrão do appsettings.json: false

        try
        {
            using var factory = new ApiFactory("Production");

            // Banco vazio e nada aplica o schema: o seed falha no startup (comportamento
            // anterior fora de Development), e nenhuma migration foi aplicada.
            var ex = Assert.ThrowsAny<Exception>(() => factory.CreateClient());

            Assert.Contains(
                Flatten(ex),
                e => e is SqliteException && e.Message.Contains("no such table", StringComparison.OrdinalIgnoreCase));
            Assert.Equal(0, AppliedMigrations(factory.DatabasePath));
        }
        finally
        {
            Environment.SetEnvironmentVariable(ApplyVariable, previous);
        }
    }
}
