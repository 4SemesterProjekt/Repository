using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace ClientAPI
{
    public class Recipe
    {
        public int RecipeID { get; set; }
        public string Name { get; set; }
        
        public ICollection<ItemRecipe> ItemRecipes { get; set; }
    }
}