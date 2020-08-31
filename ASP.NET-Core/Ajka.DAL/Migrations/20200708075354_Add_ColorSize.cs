using Microsoft.EntityFrameworkCore.Migrations;

namespace Ajka.DAL.Migrations
{
    public partial class Add_ColorSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorName",
                table: "OrderItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SizeName",
                table: "OrderItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorName",
                table: "ItemCardImage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SizeList",
                table: "ItemCard",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemCard_IsAdlerProduct",
                table: "ItemCard",
                column: "IsAdlerProduct");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemCard_IsAdlerProduct",
                table: "ItemCard");

            migrationBuilder.DropColumn(
                name: "ColorName",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "SizeName",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ColorName",
                table: "ItemCardImage");

            migrationBuilder.DropColumn(
                name: "SizeList",
                table: "ItemCard");
        }
    }
}
