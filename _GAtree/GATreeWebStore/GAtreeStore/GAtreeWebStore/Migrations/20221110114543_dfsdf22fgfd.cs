using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class dfsdf22fgfd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestDataRun_testDataRunTest_data_run_id",
                table: "GATreeTestDataRunLog");

            migrationBuilder.RenameColumn(
                name: "testDataRunTest_data_run_id",
                table: "GATreeTestDataRunLog",
                newName: "testdataRunTest_data_run_id");

            migrationBuilder.RenameIndex(
                name: "IX_GATreeTestDataRunLog_testDataRunTest_data_run_id",
                table: "GATreeTestDataRunLog",
                newName: "IX_GATreeTestDataRunLog_testdataRunTest_data_run_id");

            migrationBuilder.AddColumn<int>(
                name: "IterationNo",
                table: "GATreeTestDataRun",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxIterationNumber",
                table: "GATreeTestDataRun",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestDataRun_testdataRunTest_data_run_id",
                table: "GATreeTestDataRunLog",
                column: "testdataRunTest_data_run_id",
                principalTable: "GATreeTestDataRun",
                principalColumn: "Test_data_run_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestDataRun_testdataRunTest_data_run_id",
                table: "GATreeTestDataRunLog");

            migrationBuilder.DropColumn(
                name: "IterationNo",
                table: "GATreeTestDataRun");

            migrationBuilder.DropColumn(
                name: "MaxIterationNumber",
                table: "GATreeTestDataRun");

            migrationBuilder.RenameColumn(
                name: "testdataRunTest_data_run_id",
                table: "GATreeTestDataRunLog",
                newName: "testDataRunTest_data_run_id");

            migrationBuilder.RenameIndex(
                name: "IX_GATreeTestDataRunLog_testdataRunTest_data_run_id",
                table: "GATreeTestDataRunLog",
                newName: "IX_GATreeTestDataRunLog_testDataRunTest_data_run_id");

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestDataRun_testDataRunTest_data_run_id",
                table: "GATreeTestDataRunLog",
                column: "testDataRunTest_data_run_id",
                principalTable: "GATreeTestDataRun",
                principalColumn: "Test_data_run_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
