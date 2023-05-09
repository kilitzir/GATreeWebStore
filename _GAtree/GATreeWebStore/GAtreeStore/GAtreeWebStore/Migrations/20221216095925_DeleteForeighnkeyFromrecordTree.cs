using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class DeleteForeighnkeyFromrecordTree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATRecordOfTrees_GATreeThinClient_ThinClient_id",
                table: "GATRecordOfTrees");

            migrationBuilder.DropIndex(
                name: "IX_GATRecordOfTrees_ThinClient_id",
                table: "GATRecordOfTrees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GATRecordOfTrees_ThinClient_id",
                table: "GATRecordOfTrees",
                column: "ThinClient_id");

            migrationBuilder.AddForeignKey(
                name: "FK_GATRecordOfTrees_GATreeThinClient_ThinClient_id",
                table: "GATRecordOfTrees",
                column: "ThinClient_id",
                principalTable: "GATreeThinClient",
                principalColumn: "ThinClient_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
