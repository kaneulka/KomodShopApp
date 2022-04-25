using Microsoft.EntityFrameworkCore.Migrations;

namespace Komod.Repo.Migrations
{
    public partial class changePVCAId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyValCatArt",
                table: "PropertyValCatArt");

            migrationBuilder.DropIndex(
                name: "IX_PropertyValCatArt_ArticleId",
                table: "PropertyValCatArt");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PropertyValCatArt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyValCatArt",
                table: "PropertyValCatArt",
                columns: new[] { "ArticleId", "CategoryId", "PropertyValueId", "ProductId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyValCatArt",
                table: "PropertyValCatArt");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "PropertyValCatArt",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyValCatArt",
                table: "PropertyValCatArt",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyValCatArt_ArticleId",
                table: "PropertyValCatArt",
                column: "ArticleId");
        }
    }
}
