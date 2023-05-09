using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class UpdateTestDataRunFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Test_data_run_runStage",
                table: "GATreeTestDataRun",
                newName: "TestDataRun_runStage");

            migrationBuilder.RenameColumn(
                name: "Test_data_run_results",
                table: "GATreeTestDataRun",
                newName: "TestDataRun_results");

            migrationBuilder.RenameColumn(
                name: "Test_data_run_guid",
                table: "GATreeTestDataRun",
                newName: "TestDataRun_guid");

            migrationBuilder.RenameColumn(
                name: "Test_data_run_dateStart",
                table: "GATreeTestDataRun",
                newName: "TestDataRun_dateStart");

            migrationBuilder.RenameColumn(
                name: "Test_data_run_dateEnd",
                table: "GATreeTestDataRun",
                newName: "TestDataRun_dateEnd");

            migrationBuilder.RenameColumn(
                name: "Test_data_run_comments",
                table: "GATreeTestDataRun",
                newName: "TestDataRun_comments");

            migrationBuilder.RenameColumn(
                name: "Test_data_cycleStage",
                table: "GATreeTestDataRun",
                newName: "TestDataRun_cycleStage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TestDataRun_runStage",
                table: "GATreeTestDataRun",
                newName: "Test_data_run_runStage");

            migrationBuilder.RenameColumn(
                name: "TestDataRun_results",
                table: "GATreeTestDataRun",
                newName: "Test_data_run_results");

            migrationBuilder.RenameColumn(
                name: "TestDataRun_guid",
                table: "GATreeTestDataRun",
                newName: "Test_data_run_guid");

            migrationBuilder.RenameColumn(
                name: "TestDataRun_dateStart",
                table: "GATreeTestDataRun",
                newName: "Test_data_run_dateStart");

            migrationBuilder.RenameColumn(
                name: "TestDataRun_dateEnd",
                table: "GATreeTestDataRun",
                newName: "Test_data_run_dateEnd");

            migrationBuilder.RenameColumn(
                name: "TestDataRun_cycleStage",
                table: "GATreeTestDataRun",
                newName: "Test_data_cycleStage");

            migrationBuilder.RenameColumn(
                name: "TestDataRun_comments",
                table: "GATreeTestDataRun",
                newName: "Test_data_run_comments");
        }
    }
}
