using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class AddMoreFieldsToDataAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "TestData_correctionfactor",
                table: "GATreeTestsData",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TestData_crossoverPropability",
                table: "GATreeTestsData",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "TestData_generationGap",
                table: "GATreeTestsData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "TestData_mutationPropability",
                table: "GATreeTestsData",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "TestData_numberOfMaxGenerations",
                table: "GATreeTestsData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TestData_numberOfMaxTrees",
                table: "GATreeTestsData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestData_correctionfactor",
                table: "GATreeTestsData");

            migrationBuilder.DropColumn(
                name: "TestData_crossoverPropability",
                table: "GATreeTestsData");

            migrationBuilder.DropColumn(
                name: "TestData_generationGap",
                table: "GATreeTestsData");

            migrationBuilder.DropColumn(
                name: "TestData_mutationPropability",
                table: "GATreeTestsData");

            migrationBuilder.DropColumn(
                name: "TestData_numberOfMaxGenerations",
                table: "GATreeTestsData");

            migrationBuilder.DropColumn(
                name: "TestData_numberOfMaxTrees",
                table: "GATreeTestsData");
        }
    }
}
