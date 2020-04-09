using System.Collections.Generic;
using ApiFormat.Item;
using ApiFormat.ShadowTables;

namespace ApiFormat.Recipe
{
    public class Recipe : RecipeModel, IRecipeDTO
    {
        public List<IItemDTO> Items { get; set; }
    }
}