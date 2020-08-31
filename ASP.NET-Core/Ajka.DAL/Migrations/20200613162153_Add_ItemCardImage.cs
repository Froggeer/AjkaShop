using Microsoft.EntityFrameworkCore.Migrations;

namespace Ajka.DAL.Migrations
{
    public partial class Add_ItemCardImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdministrator",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemCardImage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCardId = table.Column<int>(nullable: false),
                    ImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCardImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemCardImage_ItemCard_ItemCardId",
                        column: x => x.ItemCardId,
                        principalTable: "ItemCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemCard_IsValid",
                table: "ItemCard",
                column: "IsValid");

            migrationBuilder.CreateIndex(
                name: "IX_Category_IsValid",
                table: "Category",
                column: "IsValid");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCardImage_ItemCardId",
                table: "ItemCardImage",
                column: "ItemCardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemCardImage");

            migrationBuilder.DropIndex(
                name: "IX_ItemCard_IsValid",
                table: "ItemCard");

            migrationBuilder.DropIndex(
                name: "IX_Category_IsValid",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IsAdministrator",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");
        }
    }
}
