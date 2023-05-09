using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class AddingRecordClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GATRecordOfTrees",
                columns: table => new
                {
                    RecordOfTrees_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordOfTrees_guid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecordOfTrees_data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordOfTrees_treeStatus = table.Column<int>(type: "int", nullable: false),
                    Test_data_run_id = table.Column<int>(type: "int", nullable: true),
                    ThinClient_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GATRecordOfTrees", x => x.RecordOfTrees_id);
                    table.ForeignKey(
                        name: "FK_GATRecordOfTrees_GATreeTestDataRun_Test_data_run_id",
                        column: x => x.Test_data_run_id,
                        principalTable: "GATreeTestDataRun",
                        principalColumn: "Test_data_run_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GATRecordOfTrees_GATreeThinClient_ThinClient_id",
                        column: x => x.ThinClient_id,
                        principalTable: "GATreeThinClient",
                        principalColumn: "ThinClient_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GATRecordOfTrees_Test_data_run_id",
                table: "GATRecordOfTrees",
                column: "Test_data_run_id");

            migrationBuilder.CreateIndex(
                name: "IX_GATRecordOfTrees_ThinClient_id",
                table: "GATRecordOfTrees",
                column: "ThinClient_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GATRecordOfTrees");
        }
    }
}
