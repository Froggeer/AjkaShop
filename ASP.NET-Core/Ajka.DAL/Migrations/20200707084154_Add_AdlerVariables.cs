using Microsoft.EntityFrameworkCore.Migrations;

namespace Ajka.DAL.Migrations
{
    public partial class Add_AdlerVariables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommodityIdentifier",
                table: "ItemCard",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdlerProduct",
                table: "ItemCard",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GroupIdentifier",
                table: "Category",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommodityIdentifier",
                table: "ItemCard");

            migrationBuilder.DropColumn(
                name: "IsAdlerProduct",
                table: "ItemCard");

            migrationBuilder.DropColumn(
                name: "GroupIdentifier",
                table: "Category");
        }
    }
}
