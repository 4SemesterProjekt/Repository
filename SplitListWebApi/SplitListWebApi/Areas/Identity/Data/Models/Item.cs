using System.Collections.Generic;
using SplitListWebApi.Models;

namespace SplitListWebApi.Areas.Identity.Data.Models
{
    public class Item
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        
        public ICollection<ItemRecipe> ItemRecipes { get; set; }
        
        public ICollection<ShoppingListItem> ShoppingListItems { get; set; }
        
        public ICollection<PantryItem> PantryItems { get; set; }
    }

    public class ItemRecipe
    {
        public int ItemID { get; set; }
        public Item item { get; set; }
        
        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
    }
}