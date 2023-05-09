using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class refreshAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Test_data_run_log_SerialNumber",
                table: "GATreeTestDataRunLog",
                newName: "Test_data_run_log_StepNumber");

            migrationBuilder.AlterColumn<string>(
                name: "TestData_guid",
                table: "GATreeTestsData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "GATreeTestDataRun",
                columns: table => new
                {
                    Test_data_run_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Test_data_run_guid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Test_data_run_dateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Test_data_run_dateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TestData_data_id = table.Column<int>(type: "int", nullable: false),
                    TestData_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GATreeTestDataRun", x => x.Test_data_run_id);
                    table.ForeignKey(
                        name: "FK_GATreeTestDataRun_GATreeTestsData_TestData_id",
                        column: x => x.TestData_id,
                        principalTable: "GATreeTestsData",
                        principalColumn: "TestData_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GATreeTestDataRun_TestData_id",
                table: "GATreeTestDataRun",
                column: "TestData_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GATreeTestDataRun");

            migrationBuilder.RenameColumn(
                name: "Test_data_run_log_StepNumber",
                table: "GATreeTestDataRunLog",
                newName: "Test_data_run_log_SerialNumber");

            migrationBuilder.AlterColumn<string>(
                name: "TestData_guid",
                table: "GATreeTestsData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
