using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApiFormat.ShadowTables;

namespace ApiFormat.Item
{
    public class ItemModel : IModel
    {
        [Key]
        public int ModelId { get; set; }
        public string Name { get; set; }

        public string Type { get; set; }
        public List<RecipeItem> RecipeItems { get; set; }
        public List<ShoppingListItem> ShoppingListItems { get; set; }
        public List<PantryItem> PantryItems { get; set; }
    }
}