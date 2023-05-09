using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class dfsdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestDataRun_GATreeTestsData_TestData_id",
                table: "GATreeTestDataRun");

            migrationBuilder.DropIndex(
                name: "IX_GATreeTestDataRun_TestData_id",
                table: "GATreeTestDataRun");

            migrationBuilder.AddColumn<int>(
                name: "TestData_id1",
                table: "GATreeTestDataRun",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GATreeTestDataRun_TestData_id1",
                table: "GATreeTestDataRun",
                column: "TestData_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestDataRun_GATreeTestsData_TestData_id1",
                table: "GATreeTestDataRun",
                column: "TestData_id1",
                principalTable: "GATreeTestsData",
                principalColumn: "TestData_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestDataRun_GATreeTestsData_TestData_id1",
                table: "GATreeTestDataRun");

            migrationBuilder.DropIndex(
                name: "IX_GATreeTestDataRun_TestData_id1",
                table: "GATreeTestDataRun");

            migrationBuilder.DropColumn(
                name: "TestData_id1",
                table: "GATreeTestDataRun");

            migrationBuilder.CreateIndex(
                name: "IX_GATreeTestDataRun_TestData_id",
                table: "GATreeTestDataRun",
                column: "TestData_id");

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestDataRun_GATreeTestsData_TestData_id",
                table: "GATreeTestDataRun",
                column: "TestData_id",
                principalTable: "GATreeTestsData",
                principalColumn: "TestData_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
