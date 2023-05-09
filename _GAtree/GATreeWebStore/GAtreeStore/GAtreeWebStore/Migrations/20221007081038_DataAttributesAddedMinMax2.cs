using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class DataAttributesAddedMinMax2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "data_DataAttributes_floatMax",
                table: "GATreeDataAttributes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "data_DataAttributes_floatMin",
                table: "GATreeDataAttributes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "data_DataAttributes_intMin",
                table: "GATreeDataAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data_DataAttributes_floatMax",
                table: "GATreeDataAttributes");

            migrationBuilder.DropColumn(
                name: "data_DataAttributes_floatMin",
                table: "GATreeDataAttributes");

            migrationBuilder.DropColumn(
                name: "data_DataAttributes_intMin",
                table: "GATreeDataAttributes");
        }
    }
}
