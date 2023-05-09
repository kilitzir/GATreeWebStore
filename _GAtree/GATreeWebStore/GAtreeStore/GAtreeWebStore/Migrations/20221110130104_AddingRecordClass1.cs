using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class AddingRecordClass1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATRecordOfTrees_GATreeTestDataRun_Test_data_run_id",
                table: "GATRecordOfTrees");

            migrationBuilder.DropForeignKey(
                name: "FK_GATRecordOfTrees_GATreeThinClient_ThinClient_id",
                table: "GATRecordOfTrees");

            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestDataRun_testdataRunTest_data_run_id",
                table: "GATreeTestDataRunLog");

            migrationBuilder.DropIndex(
                name: "IX_GATreeTestDataRunLog_testdataRunTest_data_run_id",
                table: "GATreeTestDataRunLog");

            migrationBuilder.DropColumn(
                name: "testdataRunTest_data_run_id",
                table: "GATreeTestDataRunLog");

            migrationBuilder.AlterColumn<int>(
                name: "ThinClient_id",
                table: "GATRecordOfTrees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Test_data_run_id",
                table: "GATRecordOfTrees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GATreeTestDataRunLog_Test_data_run_id",
                table: "GATreeTestDataRunLog",
                column: "Test_data_run_id");

            migrationBuilder.AddForeignKey(
                name: "FK_GATRecordOfTrees_GATreeTestDataRun_Test_data_run_id",
                table: "GATRecordOfTrees",
                column: "Test_data_run_id",
                principalTable: "GATreeTestDataRun",
                principalColumn: "Test_data_run_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GATRecordOfTrees_GATreeThinClient_ThinClient_id",
                table: "GATRecordOfTrees",
                column: "ThinClient_id",
                principalTable: "GATreeThinClient",
                principalColumn: "ThinClient_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestDataRun_Test_data_run_id",
                table: "GATreeTestDataRunLog",
                column: "Test_data_run_id",
                principalTable: "GATreeTestDataRun",
                principalColumn: "Test_data_run_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATRecordOfTrees_GATreeTestDataRun_Test_data_run_id",
                table: "GATRecordOfTrees");

            migrationBuilder.DropForeignKey(
                name: "FK_GATRecordOfTrees_GATreeThinClient_ThinClient_id",
                table: "GATRecordOfTrees");

            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestDataRun_Test_data_run_id",
                table: "GATreeTestDataRunLog");

            migrationBuilder.DropIndex(
                name: "IX_GATreeTestDataRunLog_Test_data_run_id",
                table: "GATreeTestDataRunLog");

            migrationBuilder.AddColumn<int>(
                name: "testdataRunTest_data_run_id",
                table: "GATreeTestDataRunLog",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ThinClient_id",
                table: "GATRecordOfTrees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Test_data_run_id",
                table: "GATRecordOfTrees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_GATreeTestDataRunLog_testdataRunTest_data_run_id",
                table: "GATreeTestDataRunLog",
                column: "testdataRunTest_data_run_id");

            migrationBuilder.AddForeignKey(
                name: "FK_GATRecordOfTrees_GATreeTestDataRun_Test_data_run_id",
                table: "GATRecordOfTrees",
                column: "Test_data_run_id",
                principalTable: "GATreeTestDataRun",
                principalColumn: "Test_data_run_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GATRecordOfTrees_GATreeThinClient_ThinClient_id",
                table: "GATRecordOfTrees",
                column: "ThinClient_id",
                principalTable: "GATreeThinClient",
                principalColumn: "ThinClient_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestDataRunLog_GATreeTestDataRun_testdataRunTest_data_run_id",
                table: "GATreeTestDataRunLog",
                column: "testdataRunTest_data_run_id",
                principalTable: "GATreeTestDataRun",
                principalColumn: "Test_data_run_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
