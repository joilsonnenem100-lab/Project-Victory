using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectVictory.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class SpiderCaveItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NpcBuyPrice",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Description", "Name", "NpcSellPrice" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "Validation loot item", "Rat Tail", 2 },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "Queijo levado pelos ratos.", "Cheese", 3 },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "Presa de aranha.", "Spider Fang", 8 },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "Seda de aranha gigante.", "Spider Silk", 25 }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Description", "Name", "NpcBuyPrice", "NpcSellPrice" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-0000000000a1"), "Restaura vida (suprimento de Hunt).", "Health Potion", 50, 10 },
                    { new Guid("00000000-0000-0000-0000-0000000000a2"), "Restaura mana (suprimento de Hunt).", "Mana Potion", 50, 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000000a1"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000000a2"));

            migrationBuilder.DropColumn(
                name: "NpcBuyPrice",
                table: "Items");
        }
    }
}
