using Microsoft.EntityFrameworkCore.Migrations;

namespace Ajka.DAL.Migrations
{
    public partial class Add_ItemCardsPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemCard_IsValid",
                table: "ItemCard");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "ItemCard");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ItemCard",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "ItemCard",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ItemCard_State",
                table: "ItemCard",
                column: "State");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemCard_State",
                table: "ItemCard");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ItemCard");

            migrationBuilder.DropColumn(
                name: "State",
                table: "ItemCard");

            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "ItemCard",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ItemCard_IsValid",
                table: "ItemCard",
                column: "IsValid");
        }
    }
}
