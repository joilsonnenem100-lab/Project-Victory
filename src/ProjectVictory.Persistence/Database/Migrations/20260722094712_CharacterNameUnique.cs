using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectVictory.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class CharacterNameUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Characters_Name",
                table: "Characters",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Characters_Name",
                table: "Characters");
        }
    }
}
