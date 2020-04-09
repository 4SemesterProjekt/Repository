using System.Collections.Generic;
using ApiFormat.ShadowTables;

namespace ApiFormat.Item
{
    public class Item : IItemModel, IItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
        public ICollection<RecipeItem> RecipeItems { get; set; }
        public ICollection<ShoppingListItem> ShoppingListItems { get; set; }
        public ICollection<PantryItem> PantryItems { get; set; }
    }
}