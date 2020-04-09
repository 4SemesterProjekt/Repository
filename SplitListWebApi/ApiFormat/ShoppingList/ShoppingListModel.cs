using System.Collections.Generic;
using ApiFormat.ShadowTables;

namespace ApiFormat.ShoppingList
{
    public class ShoppingListModel : IModel
    {
        public int GroupID { get; set; }
        public GroupModel Group { get; set; }

        public List<ShoppingListItem> ShoppingListItems { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}