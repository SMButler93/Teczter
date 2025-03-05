using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teczter.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefinedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestSteps_Tests_TestId",
                table: "TestSteps");

            migrationBuilder.AddForeignKey(
                name: "FK_TestSteps_Tests_TestId",
                table: "TestSteps",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestSteps_Tests_TestId",
                table: "TestSteps");

            migrationBuilder.AddForeignKey(
                name: "FK_TestSteps_Tests_TestId",
                table: "TestSteps",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id");
        }
    }
}
