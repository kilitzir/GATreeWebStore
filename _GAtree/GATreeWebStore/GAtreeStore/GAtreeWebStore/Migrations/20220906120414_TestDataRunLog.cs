using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class TestDataRunLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GATreeTestDataRunLog",
                columns: table => new
                {
                    Test_data_run_log_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Test_data_run_log_guid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Test_data_run_log_SerialNumber = table.Column<int>(type: "int", nullable: false),
                    Test_data_run_log_data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Test_data_run_log_BestResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Test_data_run_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GATreeTestDataRunLog", x => x.Test_data_run_log_id);
                    table.ForeignKey(
                        name: "FK_GATreeTestDataRunLog_GATreeTestsData_Test_data_run_id",
                        column: x => x.Test_data_run_id,
                        principalTable: "GATreeTestsData",
                        principalColumn: "TestData_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GATreeTestDataRunLog_Test_data_run_id",
                table: "GATreeTestDataRunLog",
                column: "Test_data_run_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GATreeTestDataRunLog");
        }
    }
}
