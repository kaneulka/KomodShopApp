using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Komod.Repo.Migrations
{
    public partial class addNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CountryId",
                table: "Product",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Product",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleDescrition",
                table: "Product",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Color",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleDescrition",
                table: "Category",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_CountryId",
                table: "Product",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Country_CountryId",
                table: "Product",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Country_CountryId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Product_CountryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "TitleDescrition",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "TitleDescrition",
                table: "Category");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Color",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
