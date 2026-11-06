using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectVictory.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class InventoryGoldAndItemSellPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NpcSellPrice",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "Gold",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            // PV-REWARD-010: preço de venda PROVISÓRIO do item já semeado em bancos existentes
            // (DEC-20261106-08; o seed só roda em banco vazio).
            migrationBuilder.Sql(
                "UPDATE Items SET NpcSellPrice = 10 WHERE Name = 'Training Sword';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NpcSellPrice",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "Characters");
        }
    }
}
