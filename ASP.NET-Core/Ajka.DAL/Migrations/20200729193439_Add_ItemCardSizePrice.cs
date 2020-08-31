using Microsoft.EntityFrameworkCore.Migrations;

namespace Ajka.DAL.Migrations
{
    public partial class Add_ItemCardSizePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SizeList",
                table: "ItemCard");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "OrderItem",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemCardSizePriceId",
                table: "OrderItem",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemCardSizePrice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCardId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCardSizePrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemCardSizePrice_ItemCard_ItemCardId",
                        column: x => x.ItemCardId,
                        principalTable: "ItemCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ItemCardSizePriceId",
                table: "OrderItem",
                column: "ItemCardSizePriceId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCardSizePrice_ItemCardId",
                table: "ItemCardSizePrice",
                column: "ItemCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_ItemCardSizePrice_ItemCardSizePriceId",
                table: "OrderItem",
                column: "ItemCardSizePriceId",
                principalTable: "ItemCardSizePrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_ItemCardSizePrice_ItemCardSizePriceId",
                table: "OrderItem");

            migrationBuilder.DropTable(
                name: "ItemCardSizePrice");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_ItemCardSizePriceId",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ItemCardSizePriceId",
                table: "OrderItem");

            migrationBuilder.AddColumn<string>(
                name: "SizeList",
                table: "ItemCard",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
