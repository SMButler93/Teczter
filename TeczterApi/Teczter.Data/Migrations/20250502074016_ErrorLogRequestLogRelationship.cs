using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teczter.Data.Migrations
{
    /// <inheritdoc />
    public partial class ErrorLogRequestLogRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Method",
                table: "ErrorLogs");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "ErrorLogs");

            migrationBuilder.DropColumn(
                name: "Query",
                table: "ErrorLogs");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "ErrorLogs");

            migrationBuilder.AddColumn<int>(
                name: "RequestLogId",
                table: "ErrorLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLogs_RequestLogId",
                table: "ErrorLogs",
                column: "RequestLogId",
                unique: true,
                filter: "[RequestLogId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorLogs_RequestLogs_RequestLogId",
                table: "ErrorLogs",
                column: "RequestLogId",
                principalTable: "RequestLogs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ErrorLogs_RequestLogs_RequestLogId",
                table: "ErrorLogs");

            migrationBuilder.DropIndex(
                name: "IX_ErrorLogs_RequestLogId",
                table: "ErrorLogs");

            migrationBuilder.DropColumn(
                name: "RequestLogId",
                table: "ErrorLogs");

            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "ErrorLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "ErrorLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Query",
                table: "ErrorLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusCode",
                table: "ErrorLogs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
