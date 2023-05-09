using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class RenameTestDataRunLogFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Test_data_run_log_guid",
                table: "GATreeTestDataRunLog",
                newName: "TestDataRunLog_guid");

            migrationBuilder.RenameColumn(
                name: "Test_data_run_log_data",
                table: "GATreeTestDataRunLog",
                newName: "TestDataRunLog_data");

            migrationBuilder.RenameColumn(
                name: "Test_data_run_log_StepNumber",
                table: "GATreeTestDataRunLog",
                newName: "TestDataRunLog_IterationNo");

            migrationBuilder.RenameColumn(
                name: "Test_data_run_log_BestResult",
                table: "GATreeTestDataRunLog",
                newName: "TestDataRunLog_comments");

            migrationBuilder.RenameColumn(
                name: "Test_data_run_comments",
                table: "GATreeTestDataRunLog",
                newName: "TestDataRunLog_BestResult");

            migrationBuilder.RenameColumn(
                name: "Test_data_run_log_id",
                table: "GATreeTestDataRunLog",
                newName: "TestDataRunLog_id");

            migrationBuilder.RenameColumn(
                name: "IterationNo",
                table: "GATreeTestDataRun",
                newName: "CurrentIterationNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TestDataRunLog_guid",
                table: "GATreeTestDataRunLog",
                newName: "Test_data_run_log_guid");

            migrationBuilder.RenameColumn(
                name: "TestDataRunLog_data",
                table: "GATreeTestDataRunLog",
                newName: "Test_data_run_log_data");

            migrationBuilder.RenameColumn(
                name: "TestDataRunLog_comments",
                table: "GATreeTestDataRunLog",
                newName: "Test_data_run_log_BestResult");

            migrationBuilder.RenameColumn(
                name: "TestDataRunLog_IterationNo",
                table: "GATreeTestDataRunLog",
                newName: "Test_data_run_log_StepNumber");

            migrationBuilder.RenameColumn(
                name: "TestDataRunLog_BestResult",
                table: "GATreeTestDataRunLog",
                newName: "Test_data_run_comments");

            migrationBuilder.RenameColumn(
                name: "TestDataRunLog_id",
                table: "GATreeTestDataRunLog",
                newName: "Test_data_run_log_id");

            migrationBuilder.RenameColumn(
                name: "CurrentIterationNumber",
                table: "GATreeTestDataRun",
                newName: "IterationNo");
        }
    }
}
