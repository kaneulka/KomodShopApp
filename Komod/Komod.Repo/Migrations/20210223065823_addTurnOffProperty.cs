using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Komod.Repo.Migrations
{
    public partial class addTurnOffProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TurnOff",
                table: "Property",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItem",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "OrderItem",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedDate",
                table: "Order",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ClientAddress",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientEmail",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientFIO",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientOtherPhone",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientPhone",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryMethod",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Order",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TurnOff",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "AddedDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ClientAddress",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ClientEmail",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ClientFIO",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ClientOtherPhone",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ClientPhone",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DeliveryMethod",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "UnitPrice",
                table: "OrderItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "TotalPrice",
                table: "OrderItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
