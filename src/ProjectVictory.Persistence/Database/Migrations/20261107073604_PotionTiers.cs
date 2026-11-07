using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectVictory.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class PotionTiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Description", "Name", "NpcBuyPrice", "NpcSellPrice" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-0000000000a3"), "Restaura mais vida (suprimento de Hunt).", "Strong Health Potion", 100, 20 },
                    { new Guid("00000000-0000-0000-0000-0000000000a4"), "Restaura muita vida (suprimento de Hunt).", "Great Health Potion", 190, 38 },
                    { new Guid("00000000-0000-0000-0000-0000000000a5"), "Restaura mais mana (suprimento de Hunt).", "Strong Mana Potion", 93, 18 },
                    { new Guid("00000000-0000-0000-0000-0000000000a6"), "Restaura muita mana (suprimento de Hunt).", "Great Mana Potion", 144, 28 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000000a3"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000000a4"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000000a5"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000000a6"));
        }
    }
}
