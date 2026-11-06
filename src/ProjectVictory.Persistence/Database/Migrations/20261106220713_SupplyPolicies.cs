using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectVictory.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class SupplyPolicies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupplyPolicies",
                columns: table => new
                {
                    CharacterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ActiveHealthPotionBelowPercent = table.Column<int>(type: "INTEGER", nullable: true),
                    ActiveManaPotionBelowPercent = table.Column<int>(type: "INTEGER", nullable: true),
                    HasPending = table.Column<bool>(type: "INTEGER", nullable: false),
                    PendingHealthPotionBelowPercent = table.Column<int>(type: "INTEGER", nullable: true),
                    PendingManaPotionBelowPercent = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyPolicies", x => x.CharacterId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplyPolicies");
        }
    }
}
