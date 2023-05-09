using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class AddThinCleientDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestDataRun_GATreeTestsData_TestData_id",
                table: "GATreeTestDataRun");

            migrationBuilder.AlterColumn<int>(
                name: "TestData_id",
                table: "GATreeTestDataRun",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "GATreeThinClient",
                columns: table => new
                {
                    ThinClient_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThinClient_guid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Test_data_run_id = table.Column<int>(type: "int", nullable: false),
                    testdatarunTest_data_run_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GATreeThinClient", x => x.ThinClient_id);
                    table.ForeignKey(
                        name: "FK_GATreeThinClient_GATreeTestDataRun_testdatarunTest_data_run_id",
                        column: x => x.testdatarunTest_data_run_id,
                        principalTable: "GATreeTestDataRun",
                        principalColumn: "Test_data_run_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GATreeThinClient_testdatarunTest_data_run_id",
                table: "GATreeThinClient",
                column: "testdatarunTest_data_run_id");

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestDataRun_GATreeTestsData_TestData_id",
                table: "GATreeTestDataRun",
                column: "TestData_id",
                principalTable: "GATreeTestsData",
                principalColumn: "TestData_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestDataRun_GATreeTestsData_TestData_id",
                table: "GATreeTestDataRun");

            migrationBuilder.DropTable(
                name: "GATreeThinClient");

            migrationBuilder.AlterColumn<int>(
                name: "TestData_id",
                table: "GATreeTestDataRun",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestDataRun_GATreeTestsData_TestData_id",
                table: "GATreeTestDataRun",
                column: "TestData_id",
                principalTable: "GATreeTestsData",
                principalColumn: "TestData_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
