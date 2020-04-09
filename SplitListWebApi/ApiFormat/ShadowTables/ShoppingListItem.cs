using ApiFormat.Item;
using ApiFormat.ShoppingList;

namespace ApiFormat.ShadowTables
{
    public class ShoppingListItem
    {
        public int ShoppingListID { get; set; }
        public IShoppingListModel ShoppingList { get; set; }

        public int ItemID { get; set; }
        public IItemModel Item { get; set; }

        public int Amount { get; set; }
    }
}