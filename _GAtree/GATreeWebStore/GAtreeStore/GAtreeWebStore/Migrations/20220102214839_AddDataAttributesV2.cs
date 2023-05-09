using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class AddDataAttributesV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GATreeDataAttributes",
                columns: table => new
                {
                    DataAttributes_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataAttributes_sn = table.Column<int>(type: "int", nullable: false),
                    DataAttributes_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataAttributes_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataAttributes_attributesData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataAttributes_isClass = table.Column<bool>(type: "bit", nullable: false),
                    data_DataAttributes_data_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GATreeDataAttributes", x => x.DataAttributes_Id);
                    table.ForeignKey(
                        name: "FK_GATreeDataAttributes_GATreeData_data_DataAttributes_data_ID",
                        column: x => x.data_DataAttributes_data_ID,
                        principalTable: "GATreeData",
                        principalColumn: "Data_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GATreeDataAttributes_data_DataAttributes_data_ID",
                table: "GATreeDataAttributes",
                column: "data_DataAttributes_data_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GATreeDataAttributes");
        }
    }
}
