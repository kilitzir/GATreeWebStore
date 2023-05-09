using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class RenameTestDataRunLogFieldsAndTestDataRunFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaxIterationNumber",
                table: "GATreeTestDataRun",
                newName: "TestDataRun_MaxIterationNumber");

            migrationBuilder.RenameColumn(
                name: "CurrentIterationNumber",
                table: "GATreeTestDataRun",
                newName: "TestDataRun_CurrentIterationNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TestDataRun_MaxIterationNumber",
                table: "GATreeTestDataRun",
                newName: "MaxIterationNumber");

            migrationBuilder.RenameColumn(
                name: "TestDataRun_CurrentIterationNumber",
                table: "GATreeTestDataRun",
                newName: "CurrentIterationNumber");
        }
    }
}
