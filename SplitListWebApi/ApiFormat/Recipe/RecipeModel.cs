using System.Collections.Generic;
using ApiFormat.ShadowTables;

namespace ApiFormat.Recipe
{
    public class RecipeModel : IModel
    {
        public double Id { get; set; }
        public string Name { get; set; }
        public List<RecipeItem> RecipeItems { get; set; }
    }
}