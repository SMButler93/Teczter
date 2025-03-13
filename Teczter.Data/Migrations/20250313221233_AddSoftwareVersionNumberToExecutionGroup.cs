using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teczter.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftwareVersionNumberToExecutionGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ExecutionState",
                table: "Executions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "SoftwareVersionNumber",
                table: "ExecutionGroups",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoftwareVersionNumber",
                table: "ExecutionGroups");

            migrationBuilder.AlterColumn<string>(
                name: "ExecutionState",
                table: "Executions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
