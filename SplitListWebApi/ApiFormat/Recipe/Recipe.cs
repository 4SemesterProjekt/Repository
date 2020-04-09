using System.Collections.Generic;
using ApiFormat.Item;
using ApiFormat.ShadowTables;

namespace ApiFormat.Recipe
{
    public class Recipe : IRecipeModel, IRecipeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RecipeItem> ItemRecipes { get; set; }
        public ICollection<IItemDTO> Items { get; set; }
    }
}