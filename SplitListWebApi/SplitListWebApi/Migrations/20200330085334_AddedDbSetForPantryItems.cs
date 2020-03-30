using Microsoft.EntityFrameworkCore.Migrations;

namespace SplitListWebApi.Migrations
{
    public partial class AddedDbSetForPantryItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PantryItem_Items_ItemID",
                table: "PantryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_PantryItem_Pantries_PantryID",
                table: "PantryItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PantryItem",
                table: "PantryItem");

            migrationBuilder.RenameTable(
                name: "PantryItem",
                newName: "PantryItems");

            migrationBuilder.RenameIndex(
                name: "IX_PantryItem_ItemID",
                table: "PantryItems",
                newName: "IX_PantryItems_ItemID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PantryItems",
                table: "PantryItems",
                columns: new[] { "PantryID", "ItemID" });

            migrationBuilder.AddForeignKey(
                name: "FK_PantryItems_Items_ItemID",
                table: "PantryItems",
                column: "ItemID",
                principalTable: "Items",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PantryItems_Pantries_PantryID",
                table: "PantryItems",
                column: "PantryID",
                principalTable: "Pantries",
                principalColumn: "PantryID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PantryItems_Items_ItemID",
                table: "PantryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PantryItems_Pantries_PantryID",
                table: "PantryItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PantryItems",
                table: "PantryItems");

            migrationBuilder.RenameTable(
                name: "PantryItems",
                newName: "PantryItem");

            migrationBuilder.RenameIndex(
                name: "IX_PantryItems_ItemID",
                table: "PantryItem",
                newName: "IX_PantryItem_ItemID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PantryItem",
                table: "PantryItem",
                columns: new[] { "PantryID", "ItemID" });

            migrationBuilder.AddForeignKey(
                name: "FK_PantryItem_Items_ItemID",
                table: "PantryItem",
                column: "ItemID",
                principalTable: "Items",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PantryItem_Pantries_PantryID",
                table: "PantryItem",
                column: "PantryID",
                principalTable: "Pantries",
                principalColumn: "PantryID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
