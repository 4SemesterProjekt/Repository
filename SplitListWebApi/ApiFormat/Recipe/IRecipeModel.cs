using System.Collections.Generic;
using ApiFormat.ShadowTables;

namespace ApiFormat.Recipe
{
    public interface IRecipeModel : IModel
    {
        public ICollection<RecipeItem> ItemRecipes { get; set; }
    }
}