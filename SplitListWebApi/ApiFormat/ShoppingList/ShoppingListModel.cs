using System.Collections.Generic;
using ApiFormat.Group;
using ApiFormat.ShadowTables;

namespace ApiFormat.ShoppingList
{
    public class ShoppingListModel : IModel
    {
        public double Id { get; set; }
        public string Name { get; set; }

        public double GroupModelId { get; set; }
        public GroupModel GroupModel { get; set; }

        public List<ShoppingListItem> ShoppingListItems { get; set; }
    }
}