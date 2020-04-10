using ApiFormat.Item;
using ApiFormat.ShoppingList;

namespace ApiFormat.ShadowTables
{
    public class ShoppingListItem
    {
        public double ShoppingListID { get; set; }
        public ShoppingListModel ShoppingList { get; set; }

        public double ItemID { get; set; }
        public ItemModel Item { get; set; }

        public int Amount { get; set; }
    }
}