using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teczter.Data.Migrations
{
    /// <inheritdoc />
    public partial class PillarPropertyUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pillar",
                table: "Tests",
                newName: "OwningPillar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwningPillar",
                table: "Tests",
                newName: "Pillar");
        }
    }
}
