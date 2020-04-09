using System.Collections.Generic;
using ApiFormat.ShadowTables;

namespace ApiFormat.Item
{
    public interface IItemModel : IModel
    {
        string Type { get; set; }
        ICollection<RecipeItem> RecipeItems { get; set; }
        ICollection<ShoppingListItem> ShoppingListItems { get; set; }
        ICollection<PantryItem> PantryItems { get; set; }
    }
}