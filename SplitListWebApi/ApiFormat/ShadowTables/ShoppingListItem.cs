using ApiFormat.Item;
using ApiFormat.ShoppingList;

namespace ApiFormat.ShadowTables
{
    public class ShoppingListItem
    {
        public int ShoppingListModelID { get; set; }
        public ShoppingListModel ShoppingListModel { get; set; }

        public int ItemModelID { get; set; }
        public ItemModel ItemModel { get; set; }

        public int Amount { get; set; }
    }
}