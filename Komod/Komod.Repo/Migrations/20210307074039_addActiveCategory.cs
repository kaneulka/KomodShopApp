using Microsoft.EntityFrameworkCore.Migrations;

namespace Komod.Repo.Migrations
{
    public partial class addActiveCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MainPage",
                table: "Category",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainPage",
                table: "Category");
        }
    }
}
