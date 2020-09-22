using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RedeSocialDAL.Migrations.RedeSocial
{
    public partial class Postagem1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Postagens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UsuarioId = table.Column<Guid>(nullable: false),
                    Mensagem = table.Column<string>(nullable: true),
                    URLImagem = table.Column<string>(nullable: true),
                    HoraPostagem = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postagens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Postagens");
        }
    }
}
