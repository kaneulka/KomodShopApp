using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Komod.Repo.Migrations
{
    public partial class colorAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ColorId",
                table: "Article",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ColorCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Article_ColorId",
                table: "Article",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Color_ColorId",
                table: "Article",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Color_ColorId",
                table: "Article");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropIndex(
                name: "IX_Article_ColorId",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Article");
        }
    }
}
