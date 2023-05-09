using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class dfsdf22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestDataRun_Test_data_run_id",
                table: "GATreeTestDataRunLog");

            migrationBuilder.DropIndex(
                name: "IX_GATreeTestDataRunLog_Test_data_run_id",
                table: "GATreeTestDataRunLog");

            migrationBuilder.AlterColumn<int>(
                name: "Test_data_run_id",
                table: "GATreeTestDataRunLog",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "testDataRunTest_data_run_id",
                table: "GATreeTestDataRunLog",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GATreeTestDataRunLog_testDataRunTest_data_run_id",
                table: "GATreeTestDataRunLog",
                column: "testDataRunTest_data_run_id");

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestDataRun_testDataRunTest_data_run_id",
                table: "GATreeTestDataRunLog",
                column: "testDataRunTest_data_run_id",
                principalTable: "GATreeTestDataRun",
                principalColumn: "Test_data_run_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestDataRun_testDataRunTest_data_run_id",
                table: "GATreeTestDataRunLog");

            migrationBuilder.DropIndex(
                name: "IX_GATreeTestDataRunLog_testDataRunTest_data_run_id",
                table: "GATreeTestDataRunLog");

            migrationBuilder.DropColumn(
                name: "testDataRunTest_data_run_id",
                table: "GATreeTestDataRunLog");

            migrationBuilder.AlterColumn<int>(
                name: "Test_data_run_id",
                table: "GATreeTestDataRunLog",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_GATreeTestDataRunLog_Test_data_run_id",
                table: "GATreeTestDataRunLog",
                column: "Test_data_run_id");

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestDataRun_Test_data_run_id",
                table: "GATreeTestDataRunLog",
                column: "Test_data_run_id",
                principalTable: "GATreeTestDataRun",
                principalColumn: "Test_data_run_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
