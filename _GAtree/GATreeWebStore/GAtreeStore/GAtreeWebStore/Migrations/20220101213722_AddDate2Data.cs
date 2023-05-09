using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class AddDate2Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data_Friendlyname",
                table: "GATreeData",
                newName: "Data_friendlyname");

            migrationBuilder.AddColumn<DateTime>(
                name: "Data_date",
                table: "GATreeData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data_date",
                table: "GATreeData");

            migrationBuilder.RenameColumn(
                name: "Data_friendlyname",
                table: "GATreeData",
                newName: "Data_Friendlyname");
        }
    }
}
