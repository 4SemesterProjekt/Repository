using Microsoft.EntityFrameworkCore.Migrations;

namespace SplitListWebApi.Migrations
{
    public partial class Added_ShoppingListItems_DbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItem_Items_ItemID",
                table: "ShoppingListItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItem_ShoppingLists_ShoppingListID",
                table: "ShoppingListItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingListItem",
                table: "ShoppingListItem");

            migrationBuilder.RenameTable(
                name: "ShoppingListItem",
                newName: "ShoppingListItems");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingListItem_ItemID",
                table: "ShoppingListItems",
                newName: "IX_ShoppingListItems_ItemID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingListItems",
                table: "ShoppingListItems",
                columns: new[] { "ShoppingListID", "ItemID" });

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItems_Items_ItemID",
                table: "ShoppingListItems",
                column: "ItemID",
                principalTable: "Items",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItems_ShoppingLists_ShoppingListID",
                table: "ShoppingListItems",
                column: "ShoppingListID",
                principalTable: "ShoppingLists",
                principalColumn: "ShoppingListID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItems_Items_ItemID",
                table: "ShoppingListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItems_ShoppingLists_ShoppingListID",
                table: "ShoppingListItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingListItems",
                table: "ShoppingListItems");

            migrationBuilder.RenameTable(
                name: "ShoppingListItems",
                newName: "ShoppingListItem");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingListItems_ItemID",
                table: "ShoppingListItem",
                newName: "IX_ShoppingListItem_ItemID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingListItem",
                table: "ShoppingListItem",
                columns: new[] { "ShoppingListID", "ItemID" });

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItem_Items_ItemID",
                table: "ShoppingListItem",
                column: "ItemID",
                principalTable: "Items",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItem_ShoppingLists_ShoppingListID",
                table: "ShoppingListItem",
                column: "ShoppingListID",
                principalTable: "ShoppingLists",
                principalColumn: "ShoppingListID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
