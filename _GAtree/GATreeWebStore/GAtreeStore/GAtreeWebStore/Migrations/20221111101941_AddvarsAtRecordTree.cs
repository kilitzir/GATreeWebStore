using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class AddvarsAtRecordTree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test_data_run_comments",
                table: "GATreeTestDataRunLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IterationNo",
                table: "GATRecordOfTrees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "cycleStage",
                table: "GATRecordOfTrees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "runStage",
                table: "GATRecordOfTrees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test_data_run_comments",
                table: "GATreeTestDataRunLog");

            migrationBuilder.DropColumn(
                name: "IterationNo",
                table: "GATRecordOfTrees");

            migrationBuilder.DropColumn(
                name: "cycleStage",
                table: "GATRecordOfTrees");

            migrationBuilder.DropColumn(
                name: "runStage",
                table: "GATRecordOfTrees");
        }
    }
}
