using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class AddTestDataV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Test_params",
                table: "GATreeTestsData",
                newName: "TestData_params");

            migrationBuilder.RenameColumn(
                name: "Test_notes",
                table: "GATreeTestsData",
                newName: "TestData_notes");

            migrationBuilder.RenameColumn(
                name: "Test_guid",
                table: "GATreeTestsData",
                newName: "TestData_guid");

            migrationBuilder.RenameColumn(
                name: "Test_friendlyName",
                table: "GATreeTestsData",
                newName: "TestData_friendlyName");

            migrationBuilder.RenameColumn(
                name: "Test_id",
                table: "GATreeTestsData",
                newName: "TestData_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TestData_params",
                table: "GATreeTestsData",
                newName: "Test_params");

            migrationBuilder.RenameColumn(
                name: "TestData_notes",
                table: "GATreeTestsData",
                newName: "Test_notes");

            migrationBuilder.RenameColumn(
                name: "TestData_guid",
                table: "GATreeTestsData",
                newName: "Test_guid");

            migrationBuilder.RenameColumn(
                name: "TestData_friendlyName",
                table: "GATreeTestsData",
                newName: "Test_friendlyName");

            migrationBuilder.RenameColumn(
                name: "TestData_id",
                table: "GATreeTestsData",
                newName: "Test_id");
        }
    }
}
