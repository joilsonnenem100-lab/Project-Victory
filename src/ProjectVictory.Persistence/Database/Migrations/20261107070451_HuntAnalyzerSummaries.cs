using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectVictory.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class HuntAnalyzerSummaries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HuntAnalyzerSummaries",
                columns: table => new
                {
                    HuntId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CharacterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EndReason = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    EndedAtUnixMilliseconds = table.Column<long>(type: "INTEGER", nullable: false),
                    ExperienceGained = table.Column<long>(type: "INTEGER", nullable: false),
                    ElapsedSeconds = table.Column<long>(type: "INTEGER", nullable: false),
                    ExperiencePerHour = table.Column<long>(type: "INTEGER", nullable: false),
                    Debit = table.Column<long>(type: "INTEGER", nullable: false),
                    LootValue = table.Column<long>(type: "INTEGER", nullable: false),
                    Gold = table.Column<long>(type: "INTEGER", nullable: false),
                    Profit = table.Column<long>(type: "INTEGER", nullable: false),
                    NetProfit = table.Column<long>(type: "INTEGER", nullable: false),
                    SuppliesUsed = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemsLooted = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HuntAnalyzerSummaries", x => x.HuntId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HuntAnalyzerSummaries_CharacterId_EndedAtUnixMilliseconds",
                table: "HuntAnalyzerSummaries",
                columns: new[] { "CharacterId", "EndedAtUnixMilliseconds" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HuntAnalyzerSummaries");
        }
    }
}
