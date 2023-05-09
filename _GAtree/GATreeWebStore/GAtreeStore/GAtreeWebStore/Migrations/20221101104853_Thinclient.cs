using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class Thinclient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ThinClient_isRunActive",
                table: "GATreeThinClient",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "thinClient_isActive",
                table: "GATreeThinClient",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThinClient_isRunActive",
                table: "GATreeThinClient");

            migrationBuilder.DropColumn(
                name: "thinClient_isActive",
                table: "GATreeThinClient");
        }
    }
}
