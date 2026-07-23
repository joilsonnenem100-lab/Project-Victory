using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectVictory.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class AllowMultipleCharacters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Characters_AccountId",
                table: "Characters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Characters_AccountId",
                table: "Characters",
                column: "AccountId",
                unique: true);
        }
    }
}
