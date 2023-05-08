using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenerandoExcel.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Persona",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 1, "Felipe" });

            migrationBuilder.InsertData(
                table: "Persona",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 2, "Claudia" });

            migrationBuilder.InsertData(
                table: "Persona",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 3, "Roberto" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persona");
        }
    }
}
