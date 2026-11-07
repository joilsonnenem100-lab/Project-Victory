using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectVictory.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class ServerExperienceRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServerExperienceRate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    ChangedByAccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChangedAtUnixMilliseconds = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerExperienceRate", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerExperienceRate");
        }
    }
}
