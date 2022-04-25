using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Komod.Repo.Migrations
{
    public partial class PromocodesCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promocode",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    EndOfPromocode = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promocode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromocodeArticle",
                columns: table => new
                {
                    ArticleId = table.Column<long>(nullable: false),
                    PromocodeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromocodeArticle", x => new { x.ArticleId, x.PromocodeId });
                    table.ForeignKey(
                        name: "FK_PromocodeArticle_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromocodeArticle_Promocode_PromocodeId",
                        column: x => x.PromocodeId,
                        principalTable: "Promocode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromocodeArticle_PromocodeId",
                table: "PromocodeArticle",
                column: "PromocodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromocodeArticle");

            migrationBuilder.DropTable(
                name: "Promocode");
        }
    }
}
