using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class dfsdfdfsdfj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestsData_GATreeData_TestData_data_id",
                table: "GATreeTestsData");

            migrationBuilder.DropIndex(
                name: "IX_GATreeTestsData_TestData_data_id",
                table: "GATreeTestsData");

            migrationBuilder.AddColumn<int>(
                name: "Data_id",
                table: "GATreeTestsData",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GATreeTestsData_Data_id",
                table: "GATreeTestsData",
                column: "Data_id");

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestsData_GATreeData_Data_id",
                table: "GATreeTestsData",
                column: "Data_id",
                principalTable: "GATreeData",
                principalColumn: "Data_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATreeTestsData_GATreeData_Data_id",
                table: "GATreeTestsData");

            migrationBuilder.DropIndex(
                name: "IX_GATreeTestsData_Data_id",
                table: "GATreeTestsData");

            migrationBuilder.DropColumn(
                name: "Data_id",
                table: "GATreeTestsData");

            migrationBuilder.CreateIndex(
                name: "IX_GATreeTestsData_TestData_data_id",
                table: "GATreeTestsData",
                column: "TestData_data_id");

            migrationBuilder.AddForeignKey(
                name: "FK_GATreeTestsData_GATreeData_TestData_data_id",
                table: "GATreeTestsData",
                column: "TestData_data_id",
                principalTable: "GATreeData",
                principalColumn: "Data_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
