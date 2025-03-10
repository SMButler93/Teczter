using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teczter.Data.Migrations
{
    /// <inheritdoc />
    public partial class LinkUrlRemoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Test_Urls");

            migrationBuilder.DropTable(
                name: "TestStep_Urls");

            migrationBuilder.DropTable(
                name: "LinkUrl");

            migrationBuilder.AddColumn<string>(
                name: "Urls",
                table: "TestSteps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Urls",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Urls",
                table: "TestSteps");

            migrationBuilder.DropColumn(
                name: "Urls",
                table: "Tests");

            migrationBuilder.CreateTable(
                name: "LinkUrl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkUrl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Test_Urls",
                columns: table => new
                {
                    LinkUrlsId = table.Column<int>(type: "int", nullable: false),
                    TestEntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Urls", x => new { x.LinkUrlsId, x.TestEntityId });
                    table.ForeignKey(
                        name: "FK_Test_Urls_LinkUrl_LinkUrlsId",
                        column: x => x.LinkUrlsId,
                        principalTable: "LinkUrl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Test_Urls_Tests_TestEntityId",
                        column: x => x.TestEntityId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestStep_Urls",
                columns: table => new
                {
                    LinkUrlsId = table.Column<int>(type: "int", nullable: false),
                    TestStepEntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestStep_Urls", x => new { x.LinkUrlsId, x.TestStepEntityId });
                    table.ForeignKey(
                        name: "FK_TestStep_Urls_LinkUrl_LinkUrlsId",
                        column: x => x.LinkUrlsId,
                        principalTable: "LinkUrl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestStep_Urls_TestSteps_TestStepEntityId",
                        column: x => x.TestStepEntityId,
                        principalTable: "TestSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinkUrl_Url",
                table: "LinkUrl",
                column: "Url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Test_Urls_TestEntityId",
                table: "Test_Urls",
                column: "TestEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TestStep_Urls_TestStepEntityId",
                table: "TestStep_Urls",
                column: "TestStepEntityId");
        }
    }
}
