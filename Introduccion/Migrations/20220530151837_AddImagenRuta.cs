using Microsoft.EntityFrameworkCore.Migrations;

namespace Introduccion.Migrations
{
    public partial class AddImagenRuta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imagenRuta",
                table: "Amigos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imagenRuta",
                table: "Amigos");
        }
    }
}
