using System.Collections.Generic;
using ApiFormat.ShadowTables;

namespace ApiFormat.Recipe
{
    public class RecipeModel : IModel
    {
        public List<RecipeItem> RecipeItems { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}