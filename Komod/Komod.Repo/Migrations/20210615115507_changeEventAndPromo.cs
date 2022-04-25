using Microsoft.EntityFrameworkCore.Migrations;

namespace Komod.Repo.Migrations
{
    public partial class changeEventAndPromo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Promocode",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercent",
                table: "Promocode",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PersonalUserPromo",
                table: "Promocode",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercent",
                table: "Order",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DiscountType",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromoName",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercent",
                table: "EventPromotion",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "ProductSet",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductSetName = table.Column<string>(nullable: false),
                    DiscounPercent = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSet", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSet");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Promocode");

            migrationBuilder.DropColumn(
                name: "DiscountPercent",
                table: "Promocode");

            migrationBuilder.DropColumn(
                name: "PersonalUserPromo",
                table: "Promocode");

            migrationBuilder.DropColumn(
                name: "DiscountPercent",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PromoName",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DiscountPercent",
                table: "EventPromotion");
        }
    }
}
