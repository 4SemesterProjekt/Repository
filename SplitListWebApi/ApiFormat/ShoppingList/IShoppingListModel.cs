using System.Collections.Generic;
using ApiFormat.ShadowTables;

namespace ApiFormat.ShoppingList
{
    public interface IShoppingListModel : IModel
    {
        public int GroupID { get; set; }
        public IGroupModel Group { get; set; }

        public ICollection<ShoppingListItem> ShoppingListItems { get; set; }
    }
}