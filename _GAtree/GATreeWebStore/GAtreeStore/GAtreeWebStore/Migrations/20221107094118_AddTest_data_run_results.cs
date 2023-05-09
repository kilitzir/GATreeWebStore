using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class AddTest_data_run_results : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Test_data_cycleStage",
                table: "GATreeTestDataRun",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Test_data_run_comments",
                table: "GATreeTestDataRun",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Test_data_run_results",
                table: "GATreeTestDataRun",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Test_data_run_runStage",
                table: "GATreeTestDataRun",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test_data_cycleStage",
                table: "GATreeTestDataRun");

            migrationBuilder.DropColumn(
                name: "Test_data_run_comments",
                table: "GATreeTestDataRun");

            migrationBuilder.DropColumn(
                name: "Test_data_run_results",
                table: "GATreeTestDataRun");

            migrationBuilder.DropColumn(
                name: "Test_data_run_runStage",
                table: "GATreeTestDataRun");
        }
    }
}
