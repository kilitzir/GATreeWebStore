using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class AddDataAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data_int",
                table: "GATreeData",
                newName: "Data_id");

            migrationBuilder.AlterColumn<string>(
                name: "Data_guid",
                table: "GATreeData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Data_header",
                table: "GATreeData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data_header",
                table: "GATreeData");

            migrationBuilder.RenameColumn(
                name: "Data_id",
                table: "GATreeData",
                newName: "Data_int");

            migrationBuilder.AlterColumn<string>(
                name: "Data_guid",
                table: "GATreeData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
