using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class unused : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data_guid",
                table: "GATreeDataAttributes",
                newName: "DataAttributes_guid");

            migrationBuilder.AddColumn<string>(
                name: "deleteme",
                table: "GATreeData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleteme",
                table: "GATreeData");

            migrationBuilder.RenameColumn(
                name: "DataAttributes_guid",
                table: "GATreeDataAttributes",
                newName: "Data_guid");
        }
    }
}
