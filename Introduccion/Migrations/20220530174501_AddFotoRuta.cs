using Microsoft.EntityFrameworkCore.Migrations;

namespace Introduccion.Migrations
{
    public partial class AddFotoRuta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imagenRuta",
                table: "Amigos");

            migrationBuilder.AddColumn<string>(
                name: "FotoRuta",
                table: "Amigos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoRuta",
                table: "Amigos");

            migrationBuilder.AddColumn<string>(
                name: "imagenRuta",
                table: "Amigos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
