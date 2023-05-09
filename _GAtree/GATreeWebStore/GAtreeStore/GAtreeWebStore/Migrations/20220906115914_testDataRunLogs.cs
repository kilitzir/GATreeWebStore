using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class testDataRunLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestData_dateEnd",
                table: "GATreeTestsData");

            migrationBuilder.DropColumn(
                name: "TestData_dateStart",
                table: "GATreeTestsData");

            migrationBuilder.DropColumn(
                name: "TestData_results",
                table: "GATreeTestsData");

            migrationBuilder.DropColumn(
                name: "TestData_stage",
                table: "GATreeTestsData");

            migrationBuilder.DropColumn(
                name: "TestData_trees",
                table: "GATreeTestsData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TestData_dateEnd",
                table: "GATreeTestsData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TestData_dateStart",
                table: "GATreeTestsData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TestData_results",
                table: "GATreeTestsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TestData_stage",
                table: "GATreeTestsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TestData_trees",
                table: "GATreeTestsData",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
