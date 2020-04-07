using System.Collections.Generic;

namespace SplitListWebApi.Areas.Identity.Data.Models
{
    public class Recipe
    {
        public int RecipeID { get; set; }
        public string Name { get; set; }
        
        public ICollection<ItemRecipe> ItemRecipes { get; set; }
    }
}