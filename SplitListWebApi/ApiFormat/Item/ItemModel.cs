using System.Collections.Generic;
using ApiFormat.ShadowTables;

namespace ApiFormat.Item
{
    public class ItemModel : IModel
    {
        public string Type { get; set; }
        public List<RecipeItem> RecipeItems { get; set; }
        public List<ShoppingListItem> ShoppingListItems { get; set; }
        public List<PantryItem> PantryItems { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}