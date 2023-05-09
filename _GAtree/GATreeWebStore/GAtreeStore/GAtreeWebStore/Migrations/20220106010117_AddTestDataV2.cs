using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class AddTestDataV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestsData_GATreeData_TestData_data_id",
                table: "TestsData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestsData",
                table: "TestsData");

            migrationBuilder.RenameTable(
                name: "TestsData",
                newName: "GATreeTestsData");

            migrationBuilder.RenameIndex(
                name: "IX_TestsData_TestData_data_id",
                table: "GATreeTestsData",
                newName: "IX_GATreeTestsData_TestData_data_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GATreeTestsData",
                table: "GATreeTestsData",
                column: "Test_id");

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestsData_GATreeData_TestData_data_id",
                table: "GATreeTestsData",
                column: "TestData_data_id",
                principalTable: "GATreeData",
                principalColumn: "Data_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestsData_GATreeData_TestData_data_id",
                table: "GATreeTestsData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GATreeTestsData",
                table: "GATreeTestsData");

            migrationBuilder.RenameTable(
                name: "GATreeTestsData",
                newName: "TestsData");

            migrationBuilder.RenameIndex(
                name: "IX_GATreeTestsData_TestData_data_id",
                table: "TestsData",
                newName: "IX_TestsData_TestData_data_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestsData",
                table: "TestsData",
                column: "Test_id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestsData_GATreeData_TestData_data_id",
                table: "TestsData",
                column: "TestData_data_id",
                principalTable: "GATreeData",
                principalColumn: "Data_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
