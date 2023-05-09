using Microsoft.EntityFrameworkCore.Migrations;

namespace GAtreeWebStore.Migrations
{
    public partial class AddData2DB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GATreeData",
                columns: table => new
                {
                    Data_int = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data_guid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data_Friendlyname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data_notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data_json = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GATreeData", x => x.Data_int);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GATreeData");
        }
    }
}
