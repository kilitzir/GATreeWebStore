using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class refreshAll2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestData_data_id",
                table: "GATreeTestDataRun");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestData_data_id",
                table: "GATreeTestDataRun",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
