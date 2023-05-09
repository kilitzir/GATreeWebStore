using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class RecordTreeRenameFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "runStage",
                table: "GATRecordOfTrees",
                newName: "RecordOfTrees_runStage");

            migrationBuilder.RenameColumn(
                name: "cycleStage",
                table: "GATRecordOfTrees",
                newName: "RecordOfTrees_iterationNo");

            migrationBuilder.RenameColumn(
                name: "IterationNo",
                table: "GATRecordOfTrees",
                newName: "RecordOfTrees_cycleStage");

            migrationBuilder.RenameColumn(
                name: "IsConsolidation",
                table: "GATRecordOfTrees",
                newName: "RecordOfTrees_isConsolidation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecordOfTrees_runStage",
                table: "GATRecordOfTrees",
                newName: "runStage");

            migrationBuilder.RenameColumn(
                name: "RecordOfTrees_iterationNo",
                table: "GATRecordOfTrees",
                newName: "cycleStage");

            migrationBuilder.RenameColumn(
                name: "RecordOfTrees_isConsolidation",
                table: "GATRecordOfTrees",
                newName: "IsConsolidation");

            migrationBuilder.RenameColumn(
                name: "RecordOfTrees_cycleStage",
                table: "GATRecordOfTrees",
                newName: "IterationNo");
        }
    }
}
