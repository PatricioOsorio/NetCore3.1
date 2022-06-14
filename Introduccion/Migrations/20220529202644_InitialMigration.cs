using Microsoft.EntityFrameworkCore.Migrations;

namespace Introduccion.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amigos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    Correo = table.Column<string>(nullable: false),
                    Ciudad = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amigos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Amigos",
                columns: new[] { "Id", "Ciudad", "Correo", "Nombre" },
                values: new object[] { 1, 12, "paco@gmail.com", "Paco" });

            migrationBuilder.InsertData(
                table: "Amigos",
                columns: new[] { "Id", "Ciudad", "Correo", "Nombre" },
                values: new object[] { 2, 16, "maira@gmail.com", "Maira" });

            migrationBuilder.InsertData(
                table: "Amigos",
                columns: new[] { "Id", "Ciudad", "Correo", "Nombre" },
                values: new object[] { 3, 17, "maria@gmail.com", "Maria" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amigos");
        }
    }
}
