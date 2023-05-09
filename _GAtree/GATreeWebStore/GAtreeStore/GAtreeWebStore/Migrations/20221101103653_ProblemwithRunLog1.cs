using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class ProblemwithRunLog1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestsData_Test_data_run_id",
                table: "GATreeTestDataRunLog");

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestDataRun_Test_data_run_id",
                table: "GATreeTestDataRunLog",
                column: "Test_data_run_id",
                principalTable: "GATreeTestDataRun",
                principalColumn: "Test_data_run_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestDataRun_Test_data_run_id",
                table: "GATreeTestDataRunLog");

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestsData_Test_data_run_id",
                table: "GATreeTestDataRunLog",
                column: "Test_data_run_id",
                principalTable: "GATreeTestsData",
                principalColumn: "TestData_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
