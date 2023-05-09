using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class AddTestData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestsData",
                columns: table => new
                {
                    Test_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Test_friendlyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Test_notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Test_guid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Test_params = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestData_dateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TestData_dateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TestData_stage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestData_results = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestData_trees = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestData_data_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestsData", x => x.Test_id);
                    table.ForeignKey(
                        name: "FK_TestsData_GATreeData_TestData_data_id",
                        column: x => x.TestData_data_id,
                        principalTable: "GATreeData",
                        principalColumn: "Data_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestsData_TestData_data_id",
                table: "TestsData",
                column: "TestData_data_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestsData");
        }
    }
}
