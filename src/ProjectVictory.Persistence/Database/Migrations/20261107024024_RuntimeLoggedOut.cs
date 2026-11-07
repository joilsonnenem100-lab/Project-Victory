using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectVictory.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class RuntimeLoggedOut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LoggedOut",
                table: "RuntimeStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoggedOut",
                table: "RuntimeStates");
        }
    }
}
