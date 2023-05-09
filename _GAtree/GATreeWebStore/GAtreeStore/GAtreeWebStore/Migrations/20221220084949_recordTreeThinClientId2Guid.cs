using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class recordTreeThinClientId2Guid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThinClient_id",
                table: "GATRecordOfTrees");

            migrationBuilder.AddColumn<string>(
                name: "RecordTrees_thinClientGuid",
                table: "GATRecordOfTrees",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordTrees_thinClientGuid",
                table: "GATRecordOfTrees");

            migrationBuilder.AddColumn<int>(
                name: "ThinClient_id",
                table: "GATRecordOfTrees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
